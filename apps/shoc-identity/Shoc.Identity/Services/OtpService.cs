using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.ApiCore;
using Shoc.ApiCore.Intl;
using Shoc.ApiCore.RazorEngine;
using Shoc.Core;
using Shoc.Core.Security;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Provider.Data;
using Shoc.Identity.Utility;
using Shoc.Mailing;
using Shoc.Mailing.Model;

namespace Shoc.Identity.Services;

/// <summary>
/// The one-time password service
/// </summary>
public class OtpService
{
    /// <summary>
    /// The length of OTP 
    /// </summary>
    private static readonly int OTP_LENGTH = 8;

    /// <summary>
    /// The maximum number of active OTPs
    /// </summary>
    private static readonly int MAX_ACTIVE_OTPS = 5;

    /// <summary>
    /// Give a short period of time for OTP as life time
    /// </summary>
    private static readonly TimeSpan OTP_LIFETIME = TimeSpan.FromHours(1);

    /// <summary>
    /// The self settings
    /// </summary>
    private readonly SelfSettings selfSettings;
    
    /// <summary>
    /// The user internal repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// The one-time password repository
    /// </summary>
    private readonly IOtpRepository otpRepository;

    /// <summary>
    /// The password hasher
    /// </summary>
    private readonly IPasswordHasher passwordHasher;

    /// <summary>
    /// The intl service
    /// </summary>
    private readonly IIntlService intlService;

    /// <summary>
    /// The email sender
    /// </summary>
    private readonly IEmailSender emailSender;

    /// <summary>
    /// The razor engine
    /// </summary>
    private readonly IRazorEngine razorEngine;

    /// <summary>
    /// Creates new instance of credential evaluator
    /// </summary>
    /// <param name="selfSettings">The self settings</param>
    /// <param name="userInternalRepository">The internal user repository</param>
    /// <param name="otpRepository">The one-time password repository</param>
    /// <param name="passwordHasher">The password hasher</param>
    /// <param name="intlService">The intl service</param>
    /// <param name="emailSender">The email sender</param>
    /// <param name="razorEngine">The razor engine</param>
    public OtpService(SelfSettings selfSettings,
        IUserInternalRepository userInternalRepository,
        IOtpRepository otpRepository, IPasswordHasher passwordHasher,
        IIntlService intlService, IEmailSender emailSender, IRazorEngine razorEngine)
    {
        this.selfSettings = selfSettings;
        this.userInternalRepository = userInternalRepository;
        this.otpRepository = otpRepository;
        this.passwordHasher = passwordHasher;
        this.intlService = intlService;
        this.emailSender = emailSender;
        this.razorEngine = razorEngine;
    }


    /// <summary>
    /// Request a one-time password
    /// </summary>
    /// <param name="input">The request input</param>
    /// <returns></returns>
    public async Task<OneTimePassRequestResult> Request(OneTimePassRequest input)
    {
        // empty email
        if (string.IsNullOrWhiteSpace(input.Email))
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_EMAIL).AsException();
        }

        // check delivery method
        if (input.DeliveryMethod != PasswordDeliveryMethods.EMAIL)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_DELIVERY_METHOD).AsException();
        }

        // consider default sign-in method to magic link
        if (string.IsNullOrWhiteSpace(input.SigninMethod))
        {
            input.SigninMethod = MethodTypes.MAGIC_LINK;
        }

        // invalid method of authentication
        if (input.SigninMethod != MethodTypes.OTP && input.SigninMethod != MethodTypes.MAGIC_LINK)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_AUTHENTICATION_METHOD).AsException();
        }

        // get the user by email
        var user = await this.userInternalRepository.GetByEmail(input.Email);

        // check if user exists
        if (user == null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_EMAIL).AsException();
        }

        // get all the OTPs associated with user
        var otps = await this.otpRepository.GetAll(user.Id);

        // number of active OTPs for the user
        var activeCount = otps.Count(o => o.ValidUntil >= DateTime.UtcNow);

        // check if exceeded
        if (activeCount > MAX_ACTIVE_OTPS)
        {
            throw ErrorDefinition.Validation(IdentityErrors.OTP_REQUESTS_EXCEEDED).AsException();
        }

        // generate an OTP (uppercase
        var password = Rnd.GetString(OTP_LENGTH).ToUpperInvariant();

        // hash the password
        var passwordHash = this.passwordHasher.Hash(password).AsHash();

        // generate fresh OTP sign-in link fragment
        var link = Guid.NewGuid().ToString("N");

        // create and store new one-time password
        var otp = await this.otpRepository.Create(new OneTimePassModel
        {
            UserId = user.Id,
            Target = input.Email,
            TargetType = ConfirmationTargets.EMAIL,
            DeliveryMethod = input.DeliveryMethod,
            Link = link,
            PasswordHash = passwordHash,
            ReturnUrl = input.ReturnUrl,
            Lang = input.Lang ?? this.intlService.GetDefaultLocale(),
            ValidUntil = DateTime.UtcNow.Add(OTP_LIFETIME),
            Created = DateTime.UtcNow
        });

        // send notification
        var sent = false;

        try
        {
            sent = (await this.Send(otp, password, input.SigninMethod)).Sent;
        }
        catch 
        {
            // just ignore
        }
        
        // build result
        return new OneTimePassRequestResult
        {
            DeliveryMethod = input.DeliveryMethod,
            Sent = sent
        };
    }

    /// <summary>
    /// Send the one-time password
    /// </summary>
    /// <param name="otp">The one-time password object</param>
    /// <param name="password">The one-time password in plain text</param>
    /// <param name="method">The sign-in method</param>
    /// <returns></returns>
    private async Task<EmailResult> Send(OneTimePassModel otp, string password, string method)
    {
        // the one time password vs magic link indicator
        var isOtp = method == MethodTypes.OTP;

        // stringify the creation time and make sha256 code
        var proof = otp.Created.ToString("O").ToSafeSha512();

        // build sign-in URL
        var fullUrl =
            $"{UrlExt.EnsureSlash(selfSettings.ExternalBaseAddress)}api-auth/sign-in-magic/{otp.Link}/crypto-proof/{proof}";

        // the language
        var lang = otp.Lang ?? this.intlService.GetDefaultLocale();

        // the magic link headline
        var title = this.intlService.Format(isOtp ? "signin.email.otp.title" : "signin.email.magicLink.title", lang);

        // the proper template to use
        var template = isOtp ? "~/Email/OtpEmail.cshtml" : "~/Email/MagicLinkEmail.cshtml";
        
        // build and render an email with template
        var content = await this.razorEngine.Render<dynamic>(template, new
        {
            Language = lang,
            Title = title,
            Headline = title,
            OneTimePassword = password,
            MagicLink = fullUrl
        });
        
        // send email and do not wait for the answer
        return await this.emailSender.SendAsync(new EmailMessage
        {
            Subject = title,
            To = new List<string> { otp.Target },
            Body = content,
            Resources = new List<ContentResource>
            {
                new()
                {
                    Id = "header_logo",
                    Path = AssemblyUtility.ResolveRelative("Email", "Img", "header_logo.png"),
                    Type = "image/png"
                }
            }
        });
    }
}