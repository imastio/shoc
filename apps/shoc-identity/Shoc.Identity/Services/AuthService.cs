using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Shoc.ApiCore.Intl;
using Shoc.Core;
using Shoc.Core.OpenId;
using Shoc.Identity.Config;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Flow;
using Shoc.Identity.Model.User;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The authorization service
/// </summary>
public class AuthService
{
    /// <summary>
    /// The default redirect URI
    /// </summary>
    private const string DEFAULT_REDIRECT = "/";

    /// <summary>
    /// The length of randomly generated password
    /// </summary>
    private const int USER_RANDOM_PASSWORD_LENGTH = 16;

    /// <summary>
    /// The sign-in handler
    /// </summary>
    private readonly SigninHandler signinHandler;

    /// <summary>
    /// The confirmation service
    /// </summary>
    private readonly ConfirmationService confirmationService;

    /// <summary>
    /// The user internal repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// The one-time password repository
    /// </summary>
    private readonly IOtpRepository otpRepository;

    /// <summary>
    /// The sign-on settings
    /// </summary>
    private readonly SignOnSettings signOnSettings;

    /// <summary>
    /// The user repository
    /// </summary>
    private readonly UserService userService;

    /// <summary>
    /// The credential evaluator service
    /// </summary>
    private readonly CredentialEvaluator credentialEvaluator;

    /// <summary>
    /// The identity interaction service
    /// </summary>
    private readonly IIdentityServerInteractionService identityInteraction;

    /// <summary>
    /// The intl service
    /// </summary>
    private readonly IIntlService intlService;

    /// <summary>
    /// The OIDC provider service
    /// </summary>
    private readonly OidcProviderService oidcProviderService;

    /// <summary>
    /// Creates a new instance of auth service
    /// </summary>
    /// <param name="signinHandler">The sign-in handler</param>
    /// <param name="confirmationService">The confirmation service</param>
    /// <param name="userInternalRepository">The user internal repository</param>
    /// <param name="otpRepository">The one-time password repository</param>
    /// <param name="signOnSettings">The sign-on settings</param>
    /// <param name="userService">The user service</param>
    /// <param name="credentialEvaluator">The credential evaluator service</param>
    /// <param name="identityInteraction">The identity interaction</param>
    /// <param name="intlService">The intl service</param>
    /// <param name="oidcProviderService">The oidc provider service</param>
    public AuthService(
        SigninHandler signinHandler,
        ConfirmationService confirmationService,
        IUserInternalRepository userInternalRepository,
        IOtpRepository otpRepository,
        SignOnSettings signOnSettings,
        UserService userService,
        CredentialEvaluator credentialEvaluator,
        IIdentityServerInteractionService identityInteraction,
        IIntlService intlService,
        OidcProviderService oidcProviderService)
    {
        this.signinHandler = signinHandler;
        this.confirmationService = confirmationService;
        this.userInternalRepository = userInternalRepository;
        this.otpRepository = otpRepository;
        this.signOnSettings = signOnSettings;
        this.userService = userService;
        this.credentialEvaluator = credentialEvaluator;
        this.identityInteraction = identityInteraction;
        this.intlService = intlService;
        this.oidcProviderService = oidcProviderService;
        this.credentialEvaluator = credentialEvaluator;
    }

    /// <summary>
    /// Sign-in flow implementation
    /// </summary>
    /// <param name="httpContext">The HTTP Context</param>
    /// <param name="input">The sign-in input</param>
    /// <returns></returns>
    public async Task<SignInFlowResult> SignIn(HttpContext httpContext, SignInFlowInput input)
    {
        // gets the authorization context by the return URL (authorize callback URL)
        var context = await this.identityInteraction.GetAuthorizationContextAsync(input.ReturnUrl);

        // get the user by given email and password
        var validUser = await this.credentialEvaluator.Evaluate(input.Email, input.Password);

        // check if something went wrong while signin-in
        if (validUser.Errors.Count > 0 || validUser.User == null)
        {
            await this.signinHandler.SigninFailed(input.Email);
            throw new ShocException(validUser.Errors);
        }

        // get user from result
        var user = validUser.User;

        // if signed in using OTP try complete confirmation as well
        if (validUser.Otp != null)
        {
            // delete OTP as no longer needed
            await this.otpRepository.DeleteById(validUser.Otp.Id);

            // confirm user if needed
            user = await this.confirmationService.PerformConfirmation(user, validUser.Otp.TargetType);
        }

        // the users email is not verified report early
        if (!user.EmailVerified)
        {
            throw ErrorDefinition.Validation(IdentityErrors.UNVERIFIED_EMAIL).AsException();
        }

        // do actual sign-in with given scheme
        await this.signinHandler.Signin(httpContext, new SigninPrincipal
        {
            Subject = user.Id,
            Email = user.Email,
            DisplayName = validUser.User.FullName,
            Provider = IdentityProviders.LOCAL,
            MethodType = validUser.Otp == null ? MethodTypes.PASSWORD : MethodTypes.OTP,
            MultiFactorType = MultiFactoryTypes.NONE
        });

        // try get lang parameter
        var lang = context?.Parameters.Get("lang") ?? input.Lang ?? this.intlService.GetDefaultLocale();

        // the sign-in result 
        return new SignInFlowResult
        {
            Subject = validUser.User.Id,
            ReturnUrl = string.IsNullOrWhiteSpace(input.ReturnUrl) || context == null ? "/" : input.ReturnUrl,
            ContinueFlow = context != null,
            Lang = lang
        };
    }

    /// <summary>
    /// The public sign-in context resolution endpoint
    /// </summary>
    /// <param name="input">The sign-in context input</param>
    /// <returns></returns>
    public async Task<SignInContextResponse> SignInContext(SignInContextRequest input)
    {
        // get authorization context
        var ctx = await this.identityInteraction.GetAuthorizationContextAsync(input.ReturnUrl ?? string.Empty);

        // try get lang parameter
        var lang = ctx?.Parameters.Get("lang") ?? ctx?.Parameters.Get("locale") ?? this.intlService.GetDefaultLocale();

        // return sign-in context response
        return new SignInContextResponse
        {
            Lang = lang,
            LoginHint = ctx?.LoginHint
        };
    }

    /// <summary>
    /// The public auth error resolution endpoint
    /// </summary>
    /// <param name="input">The details request input</param>
    /// <returns></returns>
    public async Task<ErrorDetailsResponse> GetErrorDetails(ErrorDetailsRequest input)
    {
        // get error context
        var error = await this.identityInteraction.GetErrorContextAsync(input.ErrorId);

        // build error response
        return new ErrorDetailsResponse
        {
            Error = error?.Error ?? "Server Error",
            ErrorDescription = error?.ErrorDescription ?? "Unknown server error",
            ClientId = error?.ClientId
        };
    }

    /// <summary>
    /// The magic-link sign in flow
    /// </summary>
    /// <param name="httpContext">The HTTP context</param>
    /// <param name="link">The link fragment</param>
    /// <param name="proof">The proof of email link</param>
    /// <returns></returns>
    public async Task<string> SignInWithLink(HttpContext httpContext, string link, string proof)
    {
        // no link to authorize
        if (string.IsNullOrWhiteSpace(link))
        {
            return DEFAULT_REDIRECT;
        }

        // try get one-time password by link
        var otp = await this.otpRepository.GetByLink(link);

        // do nothing because no link
        if (otp == null)
        {
            return DEFAULT_REDIRECT;
        }

        // create otp proof to check
        var otpProof = otp.Created.ToString("O").ToSafeSha512();

        // not equal proof, so error
        if (!string.Equals(otpProof, proof, StringComparison.InvariantCultureIgnoreCase))
        {
            return DEFAULT_REDIRECT;
        }

        // delete OTP as not needed anymore
        await this.otpRepository.DeleteById(otp.Id);

        // not valid anymore
        if (otp.ValidUntil < DateTime.UtcNow)
        {
            return DEFAULT_REDIRECT;
        }

        // try get user by id
        var user = await this.userInternalRepository.GetById(otp.UserId);

        // no user
        if (user == null)
        {
            return DEFAULT_REDIRECT;
        }

        // confirm target (email or phone) if needed
        user = await this.confirmationService.PerformConfirmation(user, otp.TargetType);

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
        var context = await this.identityInteraction.GetAuthorizationContextAsync(otp.ReturnUrl);

        // redirect with sign-in flow if context is available
        if (context != null && !string.IsNullOrWhiteSpace(otp.ReturnUrl))
        {
            return otp.ReturnUrl;
        }

        return DEFAULT_REDIRECT;
    }

    /// <summary>
    /// Sign in with external provider
    /// </summary>
    /// <param name="providerCode">The provider code</param>
    /// <param name="httpContext">The Http context</param>
    /// <returns></returns>
    public async Task<SignInFlowResult> SignInExternal(string providerCode, HttpContext httpContext)
    {
        // try getting the provider
        var provider = await this.oidcProviderService.GetByCodeOrNull(providerCode);

        // make sure provider exists
        if (provider == null)
        {
            throw ErrorDefinition.Unknown("The provider is not recognized").AsException();
        }

        // authenticate request to scheme
        var authenticateResult =
            await httpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // if authentication to external provider failed
        if (!authenticateResult.Succeeded)
        {
            throw ErrorDefinition.Unknown("Could not login to the external provider").AsException();
        }

        // get external user principal
        var principal = authenticateResult.Principal.Clone();
        
        // delete temporary cookie used during external authentication
        await httpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // get id for the user
        var userIdClaim = principal.FindFirst(KnownClaims.SUBJECT) ?? principal.FindFirst(ClaimTypes.NameIdentifier);

        // make sure user id exists
        if (userIdClaim == null)
        {
            throw ErrorDefinition.Unknown("Identity does not have a subject").AsException();
        }

        // get email
        var userEmailClaim = principal.FindFirst(KnownClaims.EMAIL) ?? principal.FindFirst(ClaimTypes.Email);

        // make sure user email exists
        if (userEmailClaim == null)
        {
            throw ErrorDefinition.Unknown("Identity does not have an email").AsException();
        }

        // get the domain name from the email
        var domainName = userEmailClaim.Value.Split('@', StringSplitOptions.RemoveEmptyEntries).Last();

        // check if domain name of email is not valid
        if (Uri.CheckHostName(domainName) != UriHostNameType.Dns)
        {
            throw ErrorDefinition.Unknown($"The domain name {domainName} is not recognized").AsException();
        }

        // now supporting only trusted providers
        if (!provider.Trusted)
        {
            throw ErrorDefinition.Unknown($"The provider {provider.Code} is not trusted").AsException();
        }

        // ensure email is lowercase
        var email = userEmailClaim.Value.ToLowerInvariant();
        
        // extract user info
        var firstName = principal.FindFirst(KnownClaims.GIVEN_NAME) ?? principal.FindFirst(ClaimTypes.GivenName);
        var lastName = principal.FindFirst(KnownClaims.FAMILY_NAME) ?? principal.FindFirst(ClaimTypes.Surname);
        var name = principal.FindFirst(KnownClaims.NAME) ?? principal.FindFirst(ClaimTypes.Name);
        var picture = principal.FindFirst(KnownClaims.PICTURE);

        // get user by email if available
        var user = await this.userInternalRepository.GetByEmail(email);

        // no such user
        if (user == null)
        {
            // create one
            var created = await this.userService.Create(new UserCreateModel
            {
                Email = email,
                EmailVerified = true,
                FirstName = firstName?.Value,
                LastName = lastName?.Value,
                FullName = name?.Value,
                PictureUri = picture?.Value,
                Password = Rnd.GetString(USER_RANDOM_PASSWORD_LENGTH),
                Type = UserTypes.EXTERNAL
            });

            // load newly created user
            user = await this.userInternalRepository.GetById(created.Id);
        }

        // something went wrong
        if (user == null)
        {
            throw ErrorDefinition.Unknown($"The user could not be created").AsException();
        }
        
        await this.signinHandler.Signin(httpContext, new SigninPrincipal
        {
            Subject = user.Id,
            Email = user.Email,
            DisplayName = user.FullName,
            Provider = providerCode,
            MethodType = MethodTypes.EXTERNAL,
            MultiFactorType = MultiFactoryTypes.NONE
        });
        
        // retrieve return URL
        var returnUrl = authenticateResult.Properties.Items["returnUrl"] ?? "~/";

        return new SignInFlowResult
        {
            Subject = user.Id,
            ContinueFlow = true,
            ReturnUrl = returnUrl
        };
    }

    /// <summary>
    /// The sign-up operation based on input
    /// </summary>
    /// <param name="httpContext">The HTTP Context</param>
    /// <param name="input">The input for sign-up</param>
    /// <returns></returns>
    public async Task<SignUpFlowResult> SignUp(HttpContext httpContext, SignUpFlowInput input)
    {
        // check if can sign-up
        if (!this.signOnSettings.SignUpEnabled)
        {
            throw ErrorDefinition.Validation(IdentityErrors.SIGNUP_DISABLED).AsException();
        }

        // gets the authorization context by the return URL (authorize callback URL)
        var context = await this.identityInteraction.GetAuthorizationContextAsync(input.ReturnUrl);

        // try get lang parameter
        var lang = context?.Parameters.Get("lang") ?? input.Lang ?? intlService.GetDefaultLocale();

        // the return url
        var returnUrl = string.IsNullOrWhiteSpace(input.ReturnUrl) || context == null ? "/" : input.ReturnUrl;

        // build a full name
        var fullname = string.IsNullOrWhiteSpace(input.FullName) ? "New User" : input.FullName;

        // try get root user if exists
        var root = await this.userInternalRepository.GetRoot();

        // use guest role as default, if no root yet create as root
        var type = root == null ? UserTypes.ROOT : UserTypes.EXTERNAL;

        // the email is not verified by default, in case if creating root email is already verified
        var emailVerified = root == null;

        // try create user based on given input
        var userCreated = await this.userService.Create(new UserCreateModel
        {
            Email = input.Email,
            EmailVerified = emailVerified,
            FullName = fullname,
            Type = type,
            UserState = UserStates.ACTIVE,
            Password = input.Password,
            Timezone = input.Timezone,
            Country = input.Country
        });

        // user is missing
        if (userCreated == null)
        {
            throw ErrorDefinition.Validation(IdentityErrors.UNKNOWN_ERROR).AsException();
        }

        // gets the user by id
        var user = await this.userInternalRepository.GetById(userCreated.Id);

        // confirmation message sent
        var confirmationSent = false;

        // need to send email verification code message
        if (!user.EmailVerified)
        {
            try
            {
                var requestResult = await this.confirmationService.Request(new ConfirmationRequest
                {
                    Target = user.Email,
                    TargetType = ConfirmationTargets.EMAIL,
                    Lang = lang ?? this.intlService.GetDefaultLocale(),
                    ReturnUrl = returnUrl
                });

                confirmationSent = requestResult.Sent;
            }
            catch (Exception)
            {
                confirmationSent = false;
            }
        }

        // sign in in case of verified email
        if (emailVerified)
        {
            // do sign-in registration was successful
            await this.signinHandler.Signin(httpContext, new SigninPrincipal
            {
                Subject = user.Id,
                Email = user.Email,
                DisplayName = user.FullName,
                Provider = IdentityProviders.LOCAL,
                MethodType = MethodTypes.PASSWORD,
                MultiFactorType = MultiFactoryTypes.NONE
            });
        }

        // build sign-up flow result
        return new SignUpFlowResult
        {
            Subject = user.Id,
            Email = user.Email,
            EmailVerified = emailVerified,
            ConfirmationSent = confirmationSent,
            ContinueFlow = context != null,
            ReturnUrl = returnUrl,
            Lang = lang
        };
    }
}