using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.ApiCore.Intl;
using Shoc.ApiCore.RazorEngine;
using Shoc.Core;
using Shoc.Core.Security;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Model.User;
using Shoc.Identity.Provider.Data;
using Shoc.Identity.Utility;
using Shoc.Mailing;
using Shoc.Mailing.Model;

namespace Shoc.Identity.Services;

/// <summary>
/// The password recovery service
/// </summary>
public class PasswordRecoveryService
{
    /// <summary>
    /// The maximum number of active password recovery codes
    /// </summary>
    private static readonly int MAX_ACTIVE_PASSWORD_RECOVERY_CODES = 4;

    /// <summary>
    /// The length of password recovery code
    /// </summary>
    private static readonly int PASSWORD_RECOVERY_CODE_LENGTH = 8;

    /// <summary>
    /// Give a short period of time for recovery code as life time
    /// </summary>
    private static readonly TimeSpan PASSWORD_RECOVERY_CODE_LIFETIME = TimeSpan.FromHours(1);

    /// <summary>
    /// The user password service
    /// </summary>
    private readonly UserPasswordService userPasswordService;

    /// <summary>
    /// The user internal repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// The confirmation code repository
    /// </summary>
    private readonly IConfirmationCodeRepository confirmationCodeRepository;

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
    /// <param name="userPasswordService">The user password service</param>
    /// <param name="userInternalRepository">The internal user repository</param>
    /// <param name="confirmationCodeRepository">The confirmation code repository</param>
    /// <param name="passwordHasher">The password hasher</param>
    /// <param name="intlService">The intl service</param>
    /// <param name="emailSender">The email sender</param>
    /// <param name="razorEngine">The razor engine</param>
    public PasswordRecoveryService(UserPasswordService userPasswordService, IUserInternalRepository userInternalRepository, IConfirmationCodeRepository confirmationCodeRepository, IPasswordHasher passwordHasher,  IIntlService intlService, IEmailSender emailSender, IRazorEngine razorEngine)
    {
        this.userPasswordService = userPasswordService;
        this.userInternalRepository = userInternalRepository;
        this.confirmationCodeRepository = confirmationCodeRepository;
        this.passwordHasher = passwordHasher;
        this.intlService = intlService;
        this.emailSender = emailSender;
        this.razorEngine = razorEngine;
    }

    /// <summary>
    /// Request a password recovery
    /// </summary>
    /// <param name="input">The request input</param>
    /// <returns></returns>
    public async Task<PasswordRecoveryRequestResult> Request(PasswordRecoveryRequest input)
    {
        // gets the user by email
        var user = await this.userInternalRepository.GetByEmail(input.Email);

        // check if no user
        if (user == null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.NO_USER).AsException();
        }

        // get existing codes for password recovery
        var existingCodes = await this.confirmationCodeRepository.GetAll(input.Email, ConfirmationTargets.PASSWORD);

        // count existing active codes
        var existingActive = existingCodes.Count(c => c.ValidUntil >= DateTime.UtcNow);

        // check if exceeded
        if (existingActive > MAX_ACTIVE_PASSWORD_RECOVERY_CODES)
        {
            throw ErrorDefinition.Validation(IdentityErrors.PASSWORD_RECOVERY_REQUESTS_EXCEEDED).AsException();
        }

        // get a fresh password recovery code (uppercase)
        var code = Rnd.GetString(PASSWORD_RECOVERY_CODE_LENGTH).ToUpperInvariant();

        // generate fresh validation link
        var link = Guid.NewGuid().ToString("N");

        // calculate the hash of code
        var codeHash = this.passwordHasher.Hash(code).AsHash();

        // build confirmation code entity
        var confirmation = await this.confirmationCodeRepository.Create(new ConfirmationCodeModel
        {
            UserId = user.Id,
            Target = input.Email,
            Lang = input.Lang,
            TargetType = ConfirmationTargets.PASSWORD,
            CodeHash = codeHash,
            Link = link,
            ValidUntil = DateTime.UtcNow.Add(PASSWORD_RECOVERY_CODE_LIFETIME),
            ReturnUrl = input.ReturnUrl,
            Created = DateTime.UtcNow
        });

        // send notification
        var sent = false;

        try
        {
            sent = (await this.Send(confirmation, code)).Sent;
        }
        catch
        {
            // just ignore
        }

        // build result
        return new PasswordRecoveryRequestResult
        {
            Sent = sent
        };
    }

    /// <summary>
    /// Process password recovery 
    /// </summary>
    /// <param name="request">The password recovery process request</param>
    /// <returns></returns>
    public async Task<PasswordRecoveryProcessResult> Process(PasswordRecoveryProcessRequest request)
    {
        // make sure password is given
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw ErrorDefinition.Validation(IdentityErrors.EMPTY_PASSWORD).AsException();
        }

        // gets the user by email
        var user = await this.userInternalRepository.GetByEmail(request.Email);

        // check if no user
        if (user == null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_EMAIL).AsException();
        }
        // get codes for the target
        var codes = await this.confirmationCodeRepository.GetAll(request.Email, ConfirmationTargets.PASSWORD);

        // choose ones that are not expired
        var validCodes = codes.Where(c => c.ValidUntil >= DateTime.UtcNow);

        // check is valid
        var isValid = validCodes.Any(c => this.passwordHasher.Check(c.CodeHash, request.Code).Verified);

        // verification failed
        if (!isValid)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_PASSWORD_RECOVERY_CODE).AsException();
        }

        // confirm otherwise
        await this.userPasswordService.UpdatePasswordById(user.Id, new UserPasswordUpdateModel
        {
            Password = request.Password,
            PasswordConfirmation = request.Password
        });

        await this.confirmationCodeRepository.DeleteAll(user.Email, ConfirmationTargets.PASSWORD);

        // build result
        return new PasswordRecoveryProcessResult
        {
            Changed = true
        };
    }
    
    /// <summary>
    /// Send the password recovery email
    /// </summary>
    /// <param name="confirmation">The confirmation object</param>
    /// <param name="code">The code to send</param>
    /// <returns></returns>
    private async Task<EmailResult> Send(ConfirmationCodeModel confirmation, string code)
    {
        // the language
        var lang = confirmation.Lang ?? this.intlService.GetDefaultLocale();

        // the confirmation headline
        var title = this.intlService.Format("recovery.email.title", confirmation.Lang);
        
        // build and render an email with template
        var content = await this.razorEngine.Render<dynamic>("~/Email/RecoverPasswordEmail.cshtml", new
        {
            Language = lang,
            Title = title,
            Headline = title,
            RecoveryCode = code
        });

        // send email and do not wait for the answer
        return await this.emailSender.SendAsync(new EmailMessage
        {
            Subject = title,
            To = new List<string> { confirmation.Target },
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