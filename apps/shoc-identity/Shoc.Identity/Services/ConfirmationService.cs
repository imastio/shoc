using System;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Http;
using Shoc.ApiCore;
using Shoc.ApiCore.Intl;
using Shoc.ApiCore.RazorEngine;
using Shoc.Core;
using Shoc.Core.Security;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Model.User;
using Shoc.Identity.Provider.Data;
using Shoc.Mailing;
using Shoc.Mailing.Model;

namespace Shoc.Identity.Services;

/// <summary>
/// The user confirmation service
/// </summary>
public class ConfirmationService
{
    /// <summary>
    /// The length of confirmation code
    /// </summary>
    private static readonly int CONFIRMATION_CODE_LENGTH = 8;

    /// <summary>
    /// The maximum number of active confirmation codes
    /// </summary>
    private static readonly int MAX_ACTIVE_CONFIRMATION_CODES = 5;

    /// <summary>
    /// Give a short period of time for confirmation code as lifetime
    /// </summary>
    private static readonly TimeSpan CONFIRMATION_CODE_LIFETIME = TimeSpan.FromHours(1);

    /// <summary>
    /// The self settings
    /// </summary>
    private readonly SelfSettings selfSettings;

    /// <summary>
    /// The sign-in handler
    /// </summary>
    private readonly SigninHandler signinHandler;

    /// <summary>
    /// The identity interaction service
    /// </summary>
    private readonly IIdentityServerInteractionService identityInteraction;

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
    /// <param name="selfSettings">The self settings</param>
    /// <param name="signinHandler">The sign-in handler</param>
    /// <param name="identityInteraction">The identity interaction service</param>
    /// <param name="userInternalRepository">The internal user repository</param>
    /// <param name="confirmationCodeRepository">The confirmation code repository</param>
    /// <param name="passwordHasher">The password hasher</param>
    /// <param name="intlService">The intl service</param>
    /// <param name="emailSender">The email sender</param>
    /// <param name="razorEngine">The razor engine</param>
    public ConfirmationService(SelfSettings selfSettings, SigninHandler signinHandler,
        IIdentityServerInteractionService identityInteraction,
        IUserInternalRepository userInternalRepository,
        IConfirmationCodeRepository confirmationCodeRepository, IPasswordHasher passwordHasher,
        IIntlService intlService, IEmailSender emailSender, IRazorEngine razorEngine)
    {
        this.selfSettings = selfSettings;
        this.signinHandler = signinHandler;
        this.identityInteraction = identityInteraction;
        this.userInternalRepository = userInternalRepository;
        this.confirmationCodeRepository = confirmationCodeRepository;
        this.passwordHasher = passwordHasher;
        this.intlService = intlService;
        this.emailSender = emailSender;
        this.razorEngine = razorEngine;
    }

    /// <summary>
    /// Request a confirmation code for the given email
    /// </summary>
    /// <param name="request">The confirmation request</param>
    /// <returns></returns>
    public async Task<ConfirmationRequestResult> Request(ConfirmationRequest request)
    {
        // check if valid request
        if (request == null || string.IsNullOrWhiteSpace(request.Target))
        {
            throw ErrorDefinition.Unknown(IdentityErrors.UNKNOWN_ERROR).AsException();
        }

        // not supported
        if (request.TargetType != ConfirmationTargets.EMAIL)
        {
            throw ErrorDefinition.Validation(IdentityErrors.UNSUPPORTED_CONFIRMATION).AsException();
        }

        // gets the user by email
        var user = await this.userInternalRepository.GetByEmail(request.Target);

        // check if no user
        if (user == null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.NO_USER).AsException();
        }

        // email is confirmed
        if (user.EmailVerified && request.TargetType == ConfirmationTargets.EMAIL)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EMAIL_ALREADY_CONFIRMED).AsException();
        }

        // request for the user
        return await this.RequestImplementation(user, request);
    }

    /// <summary>
    /// Request a confirmation code for the given email
    /// </summary>
    /// <param name="user">The target user</param>
    /// <param name="request">The request</param>
    /// <returns></returns>
    private async Task<ConfirmationRequestResult> RequestImplementation(UserInternalModel user,
        ConfirmationRequest request)
    {
        // get existing codes
        var existingCodes = await this.confirmationCodeRepository.GetAll(request.Target, request.TargetType);

        // count existing active codes
        var existingActive = existingCodes.Count(c => c.ValidUntil >= DateTime.UtcNow);

        // check if exceeded
        if (existingActive > MAX_ACTIVE_CONFIRMATION_CODES)
        {
            throw ErrorDefinition.Validation(IdentityErrors.CONFIRMATION_REQUESTS_EXCEEDED).AsException();
        }

        // get a fresh confirmation code (uppercase)
        var code = Rnd.GetString(CONFIRMATION_CODE_LENGTH).ToUpperInvariant();

        // generate fresh validation link
        var link = Guid.NewGuid().ToString("N");

        // calculate the hash of code
        var codeHash = this.passwordHasher.Hash(code).AsHash();

        // build confirmation code entity
        var confirmation = await this.confirmationCodeRepository.Create(new ConfirmationCodeModel
        {
            UserId = user.Id,
            Target = request.Target,
            Lang = request.Lang,
            TargetType = request.TargetType,
            CodeHash = codeHash,
            Link = link,
            ValidUntil = DateTime.UtcNow.Add(CONFIRMATION_CODE_LIFETIME),
            ReturnUrl = request.ReturnUrl,
            Created = DateTime.UtcNow,
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
        return new ConfirmationRequestResult { Sent = sent };
    }

    /// <summary>
    /// Send the confirmation code
    /// </summary>
    /// <param name="confirmation">The confirmation object</param>
    /// <param name="code">The code to send</param>
    /// <returns></returns>
    private async Task<EmailResult> Send(ConfirmationCodeModel confirmation, string code)
    {
        // stringify the creation time and make sha256 code
        var proof = confirmation.Created.ToString("O").ToSafeSha512();

        // build confirmation URL
        var fullUrl =
            $"{UrlExt.EnsureSlash(selfSettings.ExternalBaseAddress)}api-auth/confirmation/confirm/{confirmation.Link}/crypto-proof/{proof}";
        
        // the confirmation headline
        var title = "Account Confirmation";

        // build and render an email with template
        var content = await this.razorEngine.Render<dynamic>("~/Email/ConfirmationEmail.cshtml", new
        {
            Title = title,
            ConfirmationLink = fullUrl,
            ConfirmationCode = code
        });

        // send email and do not wait for the answer
        return await this.emailSender.SendAsync(new EmailMessage
        {
            Subject = title,
            To = [confirmation.Target],
            Body = content
        });
    }

    /// <summary>
    /// Process confirmation submit
    /// </summary>
    /// <param name="httpContext">The HTTP context</param>
    /// <param name="request">The confirmation process request</param>
    /// <returns></returns>
    public async Task<ConfirmationProcessResult> ConfirmWithRequest(HttpContext httpContext, ConfirmationProcessRequest request)
    {
        // check if valid request
        if (request == null || string.IsNullOrWhiteSpace(request.Target))
        {
            throw ErrorDefinition.Unknown(IdentityErrors.INVALID_EMAIL).AsException();
        }

        // check if valid target
        if (request.TargetType != ConfirmationTargets.EMAIL)
        {
            throw ErrorDefinition.Unknown(IdentityErrors.UNKNOWN_ERROR).AsException();
        }

        // get authorization request if given
        var authorizationRequest = await this.identityInteraction.GetAuthorizationContextAsync(request.ReturnUrl);

        // try get lang parameter
        var lang = authorizationRequest?.Parameters.Get("lang") ?? request.Lang ?? intlService.GetDefaultLocale();

        // the return url
        var returnUrl = string.IsNullOrWhiteSpace(request.ReturnUrl) || authorizationRequest == null ? "/" : request.ReturnUrl;

        // gets the user by email
        var user = await this.userInternalRepository.GetByEmail(request.Target);

        // check if no user
        if (user == null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_EMAIL).AsException();
        }

        // email is confirmed
        if (user.EmailVerified && request.TargetType == ConfirmationTargets.EMAIL)
        {
            throw ErrorDefinition.Validation(IdentityErrors.EMAIL_ALREADY_CONFIRMED).AsException();
        }

        // get codes for the target
        var codes = await this.confirmationCodeRepository.GetAll(request.Target, request.TargetType);

        // choose ones that are not expired
        var validCodes = codes.Where(c => c.ValidUntil >= DateTime.UtcNow);

        // check is valid
        var isValid = validCodes.Any(c => this.passwordHasher.Check(c.CodeHash, request.Code).Verified);

        // verification failed
        if (!isValid)
        {
            throw ErrorDefinition.Validation(IdentityErrors.INVALID_CONFIRMATION_CODE).AsException();
        }

        // confirm otherwise
        user = await this.PerformConfirmation(user, request.TargetType);

        // do sign-in registration was successful
        await this.signinHandler.Signin(httpContext, new SigninPrincipal
        {
            Subject = user.Id,
            Email = user.Email,
            DisplayName = user.FullName,
            Provider = IdentityProviders.LOCAL,
            MethodType = MethodTypes.OTP,
            MultiFactorType = MultiFactoryTypes.NONE
        });

        // build result
        return new ConfirmationProcessResult
        {
            Subject = user.Id,
            ContinueFlow = authorizationRequest != null,
            ReturnUrl = returnUrl,
            Lang = lang
        };
    }

    /// <summary>
    /// The email confirmation flow
    /// </summary>
    /// <param name="httpContext">The HTTP context</param>
    /// <param name="link">The link fragment</param>
    /// <param name="proof">The proof of email link</param>
    /// <returns></returns>
    public async Task<string> ConfirmWithLink(HttpContext httpContext, string link, string proof)
    {
        // redirect by default
        var defaultRedirect = "/";

        // no link to authorize
        if (string.IsNullOrWhiteSpace(link))
        {
            return defaultRedirect;
        }

        // try get confirmation code by link
        var code = await this.confirmationCodeRepository.GetByLink(link);

        // do nothing because no link
        if (code == null)
        {
            return defaultRedirect;
        }

        // create otp proof to check
        var codeProof = code.Created.ToString("O").ToSafeSha512();

        // not equal proof, so error
        if (!string.Equals(codeProof, proof, StringComparison.InvariantCultureIgnoreCase))
        {
            return defaultRedirect;
        }

        // delete code as not needed anymore
        var _ = await this.confirmationCodeRepository.DeleteById(code.Id);

        // not valid anymore
        if (code.ValidUntil < DateTime.UtcNow)
        {
            return defaultRedirect;
        }

        // try get user by id
        var user = await this.userInternalRepository.GetById(code.UserId);

        // no user
        if (user == null)
        {
            return defaultRedirect;
        }

        // confirm target (email or phone) if needed
        user = await this.PerformConfirmation(user, code.TargetType);

        // sign-in if valid
        await this.signinHandler.Signin(httpContext, new SigninPrincipal
        {
            Subject = user.Id,
            Email = user.Email,
            DisplayName = user.FullName,
            MethodType = MethodTypes.MAGIC_LINK,
            MultiFactorType = MultiFactoryTypes.NONE,
            Provider = IdentityProviders.LOCAL
        });

        // try get authorization context
        var authorizationRequest = await this.identityInteraction.GetAuthorizationContextAsync(code.ReturnUrl);

        // redirect with sign-in flow if context is available
        if (authorizationRequest != null && !string.IsNullOrWhiteSpace(code.ReturnUrl))
        {
            return code.ReturnUrl;
        }

        return defaultRedirect;
    }

    /// <summary>
    /// Proceed confirmation of target if needed
    /// </summary>
    /// <param name="user">The user</param>
    /// <param name="targetType">The target type</param>
    /// <returns></returns>
    public async Task<UserInternalModel> PerformConfirmation(UserInternalModel user, string targetType)
    {
        // decide if confirmation is required based on delivery type 
        var needConfirmation = targetType switch
        {
            ConfirmationTargets.EMAIL => !user.EmailVerified,
            ConfirmationTargets.PHONE => !user.PhoneVerified,
            _ => false
        };

        // use confirming action based on delivery type
        Func<UserInternalModel, Task<UserInternalModel>> confirmingAction = targetType switch
        {
            ConfirmationTargets.EMAIL => async u =>
                await this.userInternalRepository.UpdateEmailVerifiedById(u.Id, true),
            ConfirmationTargets.PHONE => async u =>
                await this.userInternalRepository.UpdatePhoneVerifiedById(u.Id, true),
            _ => Task.FromResult
        };

        // do confirmation if needed
        return needConfirmation ? await confirmingAction(user) : user;
    }
}