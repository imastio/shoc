using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Shoc.Core;
using Imast.Ext.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Shoc.ApiCore;
using Shoc.Core.Mailing;
using Shoc.Core.Security;
using Shoc.Identity.Config;
using Shoc.Identity.Model;

namespace Shoc.Identity.Services
{
    /// <summary>
    /// The authentication service
    /// </summary>
    public class AuthService
    {
        /// <summary>
        /// The default redirect path
        /// </summary>
        private const string DEFAULT_REDIRECT = "/";

        /// <summary>
        /// The length of confirmation code
        /// </summary>
        private static readonly int CONFIRMATION_CODE_LENGTH = 8;

        /// <summary>
        /// The maximum number of active confirmation codes
        /// </summary>
        private static readonly int MAX_ACTIVE_CONFIRMATION_CODES = 5;

        /// <summary>
        /// Give a short period of time for confirmation code as life time
        /// </summary>
        private static readonly TimeSpan CONFIRMATION_CODE_LIFETIME = TimeSpan.FromHours(1);

        /// <summary>
        /// The confirmation email template
        /// </summary>
        private static readonly string CONFIRMATION_TEMPLATE = ReadEmailTemplate("Templates", "confirmation.html");

        /// <summary>
        /// The self settings
        /// </summary>
        private readonly SelfSettings selfSettings;

        /// <summary>
        /// The sign-on settings
        /// </summary>
        private readonly SignOnSettings signOnSettings;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// The identity interaction service
        /// </summary>
        private readonly IIdentityServerInteractionService identityInteraction;

        /// <summary>
        /// The email sender
        /// </summary>
        private readonly IEmailSender emailSender;

        /// <summary>
        /// The password hasher
        /// </summary>
        private readonly IPasswordHasher passwordHasher;

        /// <summary>
        /// Creates new instance of authentication service
        /// </summary>
        /// <param name="selfSettings">The self settings</param>
        /// <param name="signOnSettings">The sign-on settings</param>
        /// <param name="userService">The users service</param>
        /// <param name="identityInteraction">The identity server interaction service</param>
        /// <param name="emailSender">The email sender</param>
        /// <param name="passwordHasher">The password hasher</param>
        public AuthService(SelfSettings selfSettings, SignOnSettings signOnSettings, UserService userService, IIdentityServerInteractionService identityInteraction, IEmailSender emailSender, IPasswordHasher passwordHasher)
        {
            this.selfSettings = selfSettings;
            this.signOnSettings = signOnSettings;
            this.userService = userService;
            this.identityInteraction = identityInteraction;
            this.emailSender = emailSender;
            this.passwordHasher = passwordHasher;
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
            var validUser = await this.userService.EvaluateCredentials(input.Email, input.Password);

            // check if something went wrong while signin-in
            if (validUser.Errors.Count > 0 || validUser.Value == null)
            {
                await this.userService.SignInFailed(input.Email);
                throw new ShocException(validUser.Errors);
            }

            // get user from result
            var user = validUser.Value;
            
            // the users email is not verified report early
            if (!user.EmailVerified)
            {
                throw ErrorDefinition.Validation(IdentityErrors.UNVERIFIED_EMAIL).AsException();
            }

            // do actual sign-in with given scheme
            await this.SignInImpl(httpContext, new SignInPrincipal
            {
                Subject = user.Id,
                Email = user.Email,
                DisplayName = validUser.Value.FullName,
                Provider = IdentityProviders.LOCAL,
            });
            
            // the sign-in result 
            return new SignInFlowResult
            {
                Subject = validUser.Value.Id,
                ReturnUrl = input.ReturnUrl.IsBlank() || context == null ? "/" : input.ReturnUrl,
                ContinueFlow = context != null
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
            
            // the return url
            var returnUrl = input.ReturnUrl.IsBlank() || context == null ? "/" : input.ReturnUrl;

            // validate email and password
            var validate = this.userService.ValidateEmailAndPassword(input.Email, input.Password);

            // failed validation
            if (validate.Count > 0)
            {
                throw new ShocException(validate);
            }

            // build a full name
            var fullname = input.FullName.IsBlank() ? "New User" : input.FullName;

            // try get root user if exists
            var root = await this.userService.GetRootUser();

            // use guest role as default, if no root yet create as root
            var type = root == null ? UserTypes.ROOT : UserTypes.USER;

            // the email is not verified by default, in case if creating root email is already verified
            var emailVerified = root == null;

            // try create user based on given input
            var user = await this.userService.Create(new CreateUserModel
            {
                Email = input.Email,
                EmailVerified = emailVerified,
                FullName = fullname,
                Type = type,
                Password = input.Password
            });

            // user is missing
            if (user == null)
            {
                throw ErrorDefinition.Validation(IdentityErrors.UNKNOWN_ERROR).AsException();
            }

            // confirmation message sent
            var confirmationSent = false;

            // need to send email verification code message
            if (!user.EmailVerified)
            {
                try
                {
                    var requestResult = await this.RequestConfirmation(new ConfirmationRequest
                    {
                        Email = user.Email,
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
                await this.SignInImpl(httpContext, new SignInPrincipal
                {
                    Subject = user.Id,
                    Email = user.Email,
                    DisplayName = user.FullName,
                    Provider = IdentityProviders.LOCAL
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
                ReturnUrl = returnUrl
            };
        }

        /// <summary>
        /// Request a confirmation code for the given email
        /// </summary>
        /// <param name="request">The confirmation request</param>
        /// <returns></returns>
        public async Task<ConfirmationRequestResult> RequestConfirmation(ConfirmationRequest request)
        {
            // gets the user by email
            var user = await this.userService.GetByEmail(request.Email);

            // check if no user
            if (user == null)
            {
                throw ErrorDefinition.Validation(IdentityErrors.NO_USER).AsException();
            }

            // email is confirmed
            if (user.EmailVerified)
            {
                throw ErrorDefinition.Validation(IdentityErrors.EMAIL_ALREADY_CONFIRMED).AsException();
            }

            // request for the user
            return await this.RequestConfirmation(user, request);
        }

        /// <summary>
        /// The email confirmation flow
        /// </summary>
        /// <param name="httpContext">The HTTP context</param>
        /// <param name="link">The link fragment</param>
        /// <param name="proof">The proof of email link</param>
        /// <returns></returns>
        public async Task<string> ConfirmEmail(HttpContext httpContext, string link, string proof)
        {
            // no link to authorize
            if (link.IsBlank())
            {
                return DEFAULT_REDIRECT;
            }

            // try get confirmation code by link
            var code = await this.userService.GetConfirmationByLink(link);

            // do nothing because no link
            if (code == null)
            {
                return DEFAULT_REDIRECT;
            }

            // create otp proof to check
            var codeProof = code.Created.ToString("O").ToSafeSha512();

            // not equal proof, so error
            if (!string.Equals(codeProof, proof, StringComparison.InvariantCultureIgnoreCase))
            {
                return DEFAULT_REDIRECT;
            }

            // delete code as not needed anymore
            var _ = await this.userService.DeleteConfirmation(code.Id);

            // not valid anymore
            if (code.ValidUntil < DateTime.UtcNow)
            {
                return DEFAULT_REDIRECT;
            }

            // try get user by id
            var user = await this.userService.GetById(code.UserId);

            // no user
            if (user == null)
            {
                return DEFAULT_REDIRECT;
            }

            // confirm target (email or phone) if needed
            user = await this.ProceedConfirmation(user);

            // sign-in if valid
            await this.SignInImpl(httpContext, new SignInPrincipal
            {
                Subject = user.Id,
                Email = user.Email,
                DisplayName = user.FullName,
                Provider = IdentityProviders.LOCAL
            });

            // try get authorization context
            var context = await this.identityInteraction.GetAuthorizationContextAsync(code.ReturnUrl);

            // redirect with sign-in flow if context is available
            if (context != null && code.ReturnUrl.IsNotBlank())
            {
                return code.ReturnUrl;
            }

            return DEFAULT_REDIRECT;
        }

        /// <summary>
        /// Process confirmation submit
        /// </summary>
        /// <param name="httpContext">The HTTP context</param>
        /// <param name="request">The confirmation process request</param>
        /// <returns></returns>
        public async Task<ConfirmationProcessResult> ProcessConfirmation(HttpContext httpContext, ConfirmationProcessRequest request)
        {
            // check if valid request
            if (request == null || request.Email.IsBlank())
            {
                throw ErrorDefinition.Unknown(IdentityErrors.INVALID_EMAIL).AsException();
            }

            // gets the authorization context by the return URL (authorize callback URL)
            var context = await this.identityInteraction.GetAuthorizationContextAsync(request.ReturnUrl);

            // the return url
            var returnUrl = request.ReturnUrl.IsBlank() || context == null ? "/" : request.ReturnUrl;

            // gets the user by email
            var user = await this.userService.GetByEmail(request.Email);

            // check if no user
            if (user == null)
            {
                throw ErrorDefinition.Validation(IdentityErrors.INVALID_EMAIL).AsException();
            }

            // email is confirmed
            if (user.EmailVerified)
            {
                throw ErrorDefinition.Validation(IdentityErrors.EMAIL_ALREADY_CONFIRMED).AsException();
            }

            // get codes for the target
            var codes = await this.userService.GetConfirmations(request.Email);

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
            user = await this.ProceedConfirmation(user);

            // do sign-in registration was successful
            await this.SignInImpl(httpContext, new SignInPrincipal
            {
                Subject = user.Id,
                Email = user.Email,
                DisplayName = user.FullName,
                Provider = IdentityProviders.LOCAL
            });

            // build result
            return new ConfirmationProcessResult
            {
                Subject = user.Id,
                ContinueFlow = context != null,
                ReturnUrl = returnUrl
            };
        }

        /// <summary>
        /// Proceed confirmation of target if needed
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns></returns>
        private async Task<UserModel> ProceedConfirmation(UserModel user)
        {
            // decide if confirmation is required based on delivery type 
            var needConfirmation = !user.EmailVerified;

            // do confirmation if needed
            return needConfirmation ? await this.userService.ConfirmUserEmail(user.Id) : user;
        }

        /// <summary>
        /// Request a confirmation code for the given email
        /// </summary>
        /// <param name="user">The target user</param>
        /// <param name="request">The request</param>
        /// <returns></returns>
        public async Task<ConfirmationRequestResult> RequestConfirmation(UserModel user, ConfirmationRequest request)
        {
            // get existing codes
            var existingCodes = await this.userService.GetConfirmations(request.Email);

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
            var confirmation = await this.userService.CreateConfirmation(new ConfirmationCode
            {
                UserId = user.Id,
                Email = request.Email,
                CodeHash = codeHash,
                Link = link,
                ValidUntil = DateTime.UtcNow.Add(CONFIRMATION_CODE_LIFETIME),
                ReturnUrl = request.ReturnUrl,
                Created = DateTime.UtcNow
            });

            // send notification
            var sent = await this.SendConfirmationCode(confirmation, code);

            // build result
            return new ConfirmationRequestResult { Sent = sent };
        }

        /// <summary>
        /// The implementation of sign-in to the system
        /// </summary>
        /// <param name="httpContext">The HTTP context</param>
        /// <param name="principal">The sign-in principal</param>
        /// <returns></returns>
        private async Task SignInImpl(HttpContext httpContext, SignInPrincipal principal)
        {
            // create a subject principal 
            var subject = new IdentityServerUser(principal.Subject)
            {
                DisplayName = principal.DisplayName,
                IdentityProvider = principal.Provider
            };

            // sign-in a persistent user with 14 days long session
            await httpContext.SignInAsync(subject, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(14))
            });

            await this.userService.SignInSuccess(principal, GetSignInMetadata(httpContext));
        }

        /// <summary>
        /// Send the confirmation code
        /// </summary>
        /// <param name="confirmation">The confirmation object</param>
        /// <param name="code">The code to send</param>
        /// <returns></returns>
        private async Task<bool> SendConfirmationCode(ConfirmationCode confirmation, string code)
        {
            // stringify the creation time and make sha512 code
            var proof = confirmation.Created.ToString("O").ToSafeSha512();

            // build confirmation URL
            var fullUrl = $"{selfSettings.ExternalBaseAddress}api-auth/email-confirmation/{confirmation.Link}/crypto-proof/{proof}";
            
            // set of messages for format the email
            var messages = new Dictionary<string, string>
            {
                {"confirmation_url", fullUrl},
                {"confirmation_code", code}
            };

            // format template into final message
            var content = messages.Aggregate(CONFIRMATION_TEMPLATE, (current, message) => current.Replace($"{{{message.Key}}}", message.Value));
            
            // send email and get result
            var result = await this.emailSender.SendAsync(new EmailMessage
            {
                Subject = "Please confirm your email!",
                To = new List<string> { confirmation.Email },
                Body = content,
                Resources = new List<ContentResource>()
            });

            // return result
            return result.Sent;
        }

        /// <summary>
        /// Gets the sign-in metadata
        /// </summary>
        /// <param name="httpContext">The HTTP context</param>
        /// <returns></returns>
        private static SignInMetadata GetSignInMetadata(HttpContext httpContext)
        {
            // try get user agent from request
            var userAgent = httpContext.Request.Headers.TryGetValue(HeaderNames.UserAgent, out var agent) ? agent.ToString() : "unknown";

            // get the IP address of the current connection
            var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // build metadata
            return new SignInMetadata
            {
                Ip = ip,
                UserAgent = userAgent,
                Time = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Resolves the relative path into full one
        /// </summary>
        /// <param name="parts">The path parts</param>
        /// <returns></returns>
        private static string ResolveRelative(params string[] parts)
        {
            // source directory
            var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

            // build full path
            return Path.GetFullPath(Path.Combine(parts), sourceDirectory);
        }

        /// <summary>
        /// Reads mail template into the string
        /// </summary>
        /// <returns></returns>
        private static string ReadEmailTemplate(params string[] parts)
        {
            // read content of template 
            return File.ReadAllText(ResolveRelative(parts));
        }
    }
}