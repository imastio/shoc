using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Shoc.ApiCore;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers
{
    /// <summary>
    /// The authentication controller
    /// </summary>
    [Route("api-auth")]
    [ApiController]
    [ShocExceptionHandler]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// The identity server interaction service
        /// </summary>
        private readonly IIdentityServerInteractionService interaction;

        /// <summary>
        /// The auth service
        /// </summary>
        private readonly AuthService authService;

        /// <summary>
        /// Login router controller
        /// </summary>
        /// <param name="interaction">The interaction service</param>
        /// <param name="authService">The auth service</param>
        public AuthController(IIdentityServerInteractionService interaction, AuthService authService)
        {
            this.interaction = interaction;
            this.authService = authService;
        }

        /// <summary>
        /// The public sign-in endpoint
        /// </summary>
        /// <param name="input">The sign-up input</param>
        /// <returns></returns>
        [HttpPost("sign-in")]
        public Task<SignInFlowResult> SignInFlow(SignInFlowInput input)
        {
            return this.authService.SignIn(this.HttpContext, input);
        }

        /// <summary>
        /// The public confirmation endpoint
        /// </summary>
        /// <param name="link">The link fragment</param>
        /// <param name="proof">The proof of link</param>
        /// <returns></returns>
        [HttpGet("email-confirmation/{link}/crypto-proof/{proof}")]
        public async Task<IActionResult> ConfirmEmail(string link, string proof)
        {
            return this.Redirect(await this.authService.ConfirmEmail(this.HttpContext, link, proof));
        }

        /// <summary>
        /// The public confirmation code request
        /// </summary>
        /// <param name="input">The code request input</param>
        /// <returns></returns>
        [HttpPost("request-confirmation-code")]
        public Task<ConfirmationRequestResult> RequestConfirmationCode(ConfirmationRequest input)
        {
            return this.authService.RequestConfirmation(input);
        }

        /// <summary>
        /// The public to process confirmation request
        /// </summary>
        /// <param name="input">The confirmation input</param>
        /// <returns></returns>
        [HttpPost("confirm-account")]
        public Task<ConfirmationProcessResult> ConfirmAccount(ConfirmationProcessRequest input)
        {
            return this.authService.ProcessConfirmation(this.HttpContext, input);
        }

        /// <summary>
        /// The public sign-up endpoint
        /// </summary>
        /// <param name="input">The sign-up input</param>
        /// <returns></returns>
        [HttpPost("sign-up")]
        public Task<SignUpFlowResult> SignUpFlow(SignUpFlowInput input)
        {
            return this.authService.SignUp(this.HttpContext, input);
        }

        /// <summary>
        /// The public sign-out endpoint
        /// </summary>
        /// <param name="input">The sing-out input</param>
        /// <returns></returns>
        [HttpPost("sign-out")]
        public async Task<SignOutFlowResult> SignOutFlow(SignOutFlowInput input)
        {
            // get the context
            var context = await this.interaction.GetLogoutContextAsync(input.LogoutId);

            // valid context is required but not found
            if (input.RequireValidContext && context == null)
            {
                throw ErrorDefinition.Validation(IdentityErrors.HOTLINK_SIGNOUT).AsException();
            }

            // sign out
            await this.HttpContext.SignOutAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme);

            // get signout iframe uri
            var signoutIframeUri = string.IsNullOrWhiteSpace(context?.SignOutIFrameUrl) ? null : new Uri(context.SignOutIFrameUrl);

            // return flow result
            return new SignOutFlowResult
            {
                SignOutIframeUrl = signoutIframeUri?.PathAndQuery,
                PostLogoutRedirectUri = context?.PostLogoutRedirectUri ?? "/",
                ContinueFlow = context != null
            };
        }
    }
}
