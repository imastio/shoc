using System.Threading.Tasks;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.Core.OpenId;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The current session controller
/// </summary>
[Route("api/session")]
[ApiController]
[ShocExceptionHandler]
[Authorize(AuthenticationSchemes = IdentityServerConstants.DefaultCookieAuthenticationScheme)]
[AllowAnonymous]
public class SessionController : ControllerBase
{
    /// <summary>
    /// The current session service
    /// </summary>
    private readonly CurrentSessionService currentSessionService;

    /// <summary>
    /// Creates new instance of current session controller
    /// </summary>
    public SessionController(CurrentSessionService currentSessionService)
    {
        this.currentSessionService = currentSessionService;
    }

    /// <summary>
    /// Gets the current session state
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<object> Get()
    {
        // try resolve subject
        var subject = this.HttpContext.User.FindFirst(claim => claim.Type == KnownClaims.SUBJECT)?.Value;

        // get the session state
        return this.currentSessionService.Get(subject);
    }
}
