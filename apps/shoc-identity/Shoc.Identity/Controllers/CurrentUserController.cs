using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Identity.Model.User;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The current user controller
/// </summary>
[Route("api/current-user")]
[ApiController]
[BearerOnly]
[ShocExceptionHandler]
[AuthorizeMinUserType(UserTypes.EXTERNAL)]
public class CurrentEmployeeController : ControllerBase
{
    /// <summary>
    /// The current user service
    /// </summary>
    private readonly CurrentUserService currentUserService;

    /// <summary>
    /// Creates new instance of current user controller
    /// </summary>
    public CurrentEmployeeController(CurrentUserService currentUserService)
    {
        this.currentUserService = currentUserService;
    }

    /// <summary>
    /// Gets the current users effective access list
    /// </summary>
    /// <returns></returns>
    [HttpGet("effective-accesses")]
    public Task<IEnumerable<string>> GetEffectiveAccesses()
    {
        return this.currentUserService.GetEffectiveAccesses(this.HttpContext.GetPrincipal());
    }
}
