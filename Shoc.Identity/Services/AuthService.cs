using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using Shoc.Core;
using Imast.Ext.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Shoc.Identity.Model;

namespace Shoc.Identity.Services
{
    /// <summary>
    /// The authentication service
    /// </summary>
    public class AuthService
    {
        /// <summary>
        /// The length for generating random password
        /// </summary>
        private static readonly int USER_RANDOM_PASSWORD_LENGTH = 20;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// The identity interaction service
        /// </summary>
        private readonly IIdentityServerInteractionService identityInteraction;

        /// <summary>
        /// Creates new instance of authentication service
        /// </summary>
        /// <param name="userService">The users service</param>
        /// <param name="identityInteraction">The identity server interaction service</param>
        public AuthService( UserService userService, IIdentityServerInteractionService identityInteraction)
        {
            this.userService = userService;
            this.identityInteraction = identityInteraction;
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
        /// Sign in with Google external provider
        /// </summary>
        /// <param name="httpContext">The Http context</param>
        /// <returns></returns>
        public async Task<SignInFlowResult> SignInGoogle(HttpContext httpContext)
        {
            // get root user
            var root = await this.userService.GetRootUser();

            // make sure root exists
            if (root == null)
            {
                throw new Exception("No root");
            }

            // authenticate request to scheme
            var authenticateResult = await httpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

            // if authentication to external provider failed
            if (!authenticateResult.Succeeded)
            {
                throw new Exception("Login error");
            }

            // get external user principal
            var principal = authenticateResult.Principal;

            // get id for the user
            var userIdClaim = principal.FindFirst(KnownClaims.SUBJECT) ?? principal.FindFirst(ClaimTypes.NameIdentifier);

            // make sure user id exists
            if (userIdClaim == null)
            {
                throw ErrorDefinition.Unknown("Unknown userid").AsException();
            }

            // get email
            var userEmailClaim = principal.FindFirst(ClaimTypes.Email);

            // make sure user email exists
            if (userEmailClaim == null)
            {
                throw ErrorDefinition.Unknown("Unknown email").AsException();
            }

            // get the external provider name
            var provider = authenticateResult.Properties.Items["provider"];

            // make sure the provider is the expected one
            if (provider != IdentityProviders.GOOGLE)
            {
                ErrorDefinition.Unknown().Throw();
            }

            // get user by email from our db
            // if missing, then create
            var user = await this.userService.GetByEmail(userEmailClaim.Value) ?? await this.userService.Create(new CreateUserModel
            {
                Email = userEmailClaim.Value,
                EmailVerified = true,
                FirstName = principal.FindFirst(ClaimTypes.GivenName)?.Value,
                LastName = principal.FindFirst(ClaimTypes.Surname)?.Value,
                FullName = principal.FindFirst(ClaimTypes.Name)?.Value,
                Password = Rnd.GetString(USER_RANDOM_PASSWORD_LENGTH),
                Type = UserTypes.USER
            });

            // get external user by email and provider
            var externalUser = await this.userService.GetExternalByEmailAndProvider(user.Email, provider);

            // if external user does exist, then create
            if (externalUser == null)
            {
                await this.userService.CreateExternal(new CreateExternalUserModel
                {
                    ExternalId = userIdClaim.Value,
                    UserId = user.Id,
                    Provider = provider.ToLowerInvariant(),
                    Email = user.Email
                });
            }

            // do actual sign-in with given scheme
            await this.SignInImpl(httpContext, new SignInPrincipal
            {
                Subject = user.Id,
                Email = user.Email,
                DisplayName = user.FullName,
                Provider = IdentityProviders.GOOGLE
            });

            // delete temporary cookie used during external authentication
            await httpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

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
    }
}