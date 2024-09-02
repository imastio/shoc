using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shoc.ApiCore;
using Shoc.Registry.Services;

namespace Shoc.Registry.Controllers;

/// <summary>
/// The authentication controller
/// </summary>
[Route("api/authentication/{workspaceName?}/{registryName}")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    /// <summary>
    /// The JWK service
    /// </summary>
    private readonly JwkService jwkService;
    
    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    public AuthenticationController(JwkService jwkService)
    {
        this.jwkService = jwkService;
    }
    
    /// <summary>
    /// Gets all JWKs
    /// </summary>
    /// <returns></returns>
    [HttpGet("jwks")]
    [ShocExceptionHandler]
    public Task<IEnumerable<JsonWebKey>> GetAll(string workspaceName, string registryName)
    {
        return this.jwkService.GetJwks(workspaceName, registryName);
    }
}