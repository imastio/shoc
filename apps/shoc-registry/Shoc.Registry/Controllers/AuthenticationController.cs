using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.Registry.Model.TokenSpec;
using Shoc.Registry.Services;
using Shoc.Registry.Utility;

namespace Shoc.Registry.Controllers;

/// <summary>
/// The authentication controller
/// </summary>
[Route("api/authentication/{workspaceName}/{registryName}")]
[ApiController]
[AllowAnonymous]
[ShocExceptionHandler]
public class AuthenticationController : ControllerBase
{
    /// <summary>
    /// The JWK service
    /// </summary>
    private readonly JwkService jwkService;

    /// <summary>
    /// The authentication service
    /// </summary>
    private readonly TokenAuthenticationService authenticationService;
    
    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    public AuthenticationController(JwkService jwkService, TokenAuthenticationService authenticationService)
    {
        this.jwkService = jwkService;
        this.authenticationService = authenticationService;
    }
    
    /// <summary>
    /// Gets all JWKs
    /// </summary>
    /// <returns></returns>
    [HttpGet("jwks")]
    public async Task<object> GetAll(string workspaceName, string registryName)
    {
        // load JWKs
        var jwks = await this.jwkService.GetJwks(workspaceName, registryName);
        
        return new
        {
            Keys = jwks 
        };
    }
    
    /// <summary>
    /// Generate the token based on token flow
    /// </summary>
    /// <returns></returns>
    [HttpGet("token")]
    public async Task<IActionResult> GetToken(
        string workspaceName, 
        string registryName, 
        [FromQuery] string service, 
        [FromQuery] string[] scope, 
        [FromQuery(Name = "client_id")] string clientId, 
        [FromQuery(Name = "offline_token")] bool offlineToken)
    {
        // get basic credentials if available
        var credentials = this.HttpContext.Request.Headers.GetBasicCredentials();

        // build the token request
        var tokenRequest = new TokenRequestSpec
        {
            GrantType = "password",
            Service = service,
            ClientId = clientId,
            AccessType = offlineToken ? "offline" : "online",
            Scope = string.Join(' ', scope),
            RefreshToken = null,
            Username = credentials.Username,
            Password = credentials.Password
        };

        // generate the response based on the input 
        var response = await this.authenticationService.GetToken(workspaceName, registryName, tokenRequest);

        // handle the success case
        if (response is TokenResponseSpec responseSpec)
        {
            return this.Ok(responseSpec);
        }
        
        // return unauthorized error
        return this.Unauthorized(response);
    }
    
    /// <summary>
    /// Generate the token based on OAuth2 flow
    /// </summary>
    /// <returns></returns>
    [HttpPost("token")]
    public async Task<IActionResult> GetToken(string workspaceName, string registryName)
    {
        // get the form from the request
        var form = this.HttpContext.Request.Form;
        
        // build the token request
        var tokenRequest = new TokenRequestSpec
        {
            GrantType = form.TryGetValue("grant_type", out var grantType) ? grantType.FirstOrDefault() : null,
            Service = form.TryGetValue("service", out var service) ? service.FirstOrDefault() : null,
            ClientId = form.TryGetValue("client_id", out var clientId) ? clientId.FirstOrDefault() : null,
            AccessType = form.TryGetValue("access_type", out var accessType) ? accessType.FirstOrDefault() : null,
            Scope = form.TryGetValue("scope", out var scope) ? scope.FirstOrDefault() : null,
            RefreshToken = form.TryGetValue("refresh_token", out var refreshToken) ? refreshToken.FirstOrDefault() : null,
            Username = form.TryGetValue("username", out var username) ? username.FirstOrDefault() : null,
            Password = form.TryGetValue("password", out var password) ? password.FirstOrDefault() : null
        };

        // generate the response based on the input 
        var response = await this.authenticationService.GetToken(workspaceName, registryName, tokenRequest);

        // handle the success case
        if (response is TokenResponseSpec responseSpec)
        {
            return this.Ok(responseSpec);
        }
        
        // return unauthorized error
        return this.Unauthorized(response);
    }
}