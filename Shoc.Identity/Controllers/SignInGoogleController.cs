using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Shoc.ApiCore;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers
{
    /// <summary>
    /// The google sign-in controller
    /// </summary>
    [Route("sign-in/google")]
    [AllowAnonymous]
    [ApiController]
    [ShocExceptionHandler]
    public class SignInGoogleController : ControllerBase
    {
        /// <summary>
        /// The identity server interaction service
        /// </summary>
        private readonly IIdentityServerInteractionService interaction;

        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly AuthService authService;

        /// <summary>
        /// Login router controller
        /// </summary>
        /// <param name="interaction">The interaction service</param>
        /// <param name="authService">The users service</param>
        public SignInGoogleController(IIdentityServerInteractionService interaction, AuthService authService)
        {
            this.interaction = interaction;
            this.authService = authService;
        }

        /// <summary>
        /// The public sign-in endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SignInFlow([FromQuery] string returnUrl)
        {
            // set redirect uri
            var redirectUri = $"{this.Request.Scheme}://{this.Request.Host.Value}/sign-in/google/callback";

            // make sure return url is valid
            if (!this.interaction.IsValidReturnUrl(returnUrl))
            {
                ErrorDefinition.Validation("Invalid return url").Throw();
            }

            // set props for external oidc provider
            var props = new AuthenticationProperties
            {
                RedirectUri = redirectUri,
                Items =
                {
                    { "returnUrl", returnUrl},
                    { "provider", IdentityProviders.GOOGLE}
                }
            };

            return this.Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// The public callback endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            var result = await this.authService.SignInGoogle(this.HttpContext);

            return Redirect(result.ReturnUrl);
        }
    }
}
