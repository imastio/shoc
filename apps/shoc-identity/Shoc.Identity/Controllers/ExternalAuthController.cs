using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Shoc.ApiCore;
using Shoc.Identity.Config.Oidc;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The external sign-in controller
/// </summary>
[Route("api-auth/external")]
[AllowAnonymous]
[ApiController]
[ShocExceptionHandler]
public class SignInGoogleController : ControllerBase
{
    /// <summary>
    /// The authentication service
    /// </summary>
    private readonly AuthService authService;

    /// <summary>
    /// The oidc provider service
    /// </summary>
    private readonly OidcProviderService oidcProviderService;

    /// <summary>
    /// Login router controller
    /// </summary>
    /// <param name="authService">The users service</param>
    /// <param name="oidcProviderService">The oidc provider service</param>
    public SignInGoogleController(AuthService authService, OidcProviderService oidcProviderService)
    {
        this.authService = authService;
        this.oidcProviderService = oidcProviderService;
    }

    /// <summary>
    /// The public sign-in endpoint for external provider
    /// </summary>
    /// <returns></returns>
    [HttpGet("{providerCode}")]
    public async Task<IActionResult> SignInFlow(string providerCode, [FromQuery] string returnUrl)
    {
        // set redirect uri
        var redirectUri = $"{this.Request.Scheme}://{this.Request.Host.Value}/api-auth/external/{providerCode}/callback";

        // ensure we have return url
        returnUrl = string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl;

        // try getting the provider
        var provider = await this.oidcProviderService.GetByCodeOrNull(providerCode);

        // the provider is unknown
        if (provider == null)
        {
            return this.Redirect("/error");
        }
        
        // set props for external oidc provider
        var props = new AuthenticationProperties
        {
            RedirectUri = redirectUri,
            Items =
            {
                { "returnUrl", returnUrl},
                { OidcProviderConstants.PROVIDER_CODE_KEY, providerCode}
            }
        };

        try
        {
            return this.Challenge(props, OidcProviderConstants.DYNAMIC_OIDC_SCHEME);
        }
        catch (Exception)
        {
            return this.Redirect("/error");
        }
    }

    /// <summary>
    /// The public callback endpoint for external provider
    /// </summary>
    /// <returns></returns>
    [HttpGet("{providerCode}/callback")]
    public async Task<IActionResult> Callback(string providerCode)
    {
        try
        {
            var result = await this.authService.SignInExternal(providerCode, this.HttpContext);

            return Redirect(result.ReturnUrl);
        }
        catch (Exception)
        {
            return this.Redirect("/error");
        }
        
    }
}