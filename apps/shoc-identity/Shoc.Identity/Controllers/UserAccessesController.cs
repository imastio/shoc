using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Identity.Model;
using Shoc.Identity.Model.User;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The users controller
/// </summary>
[Route("api/users/{userId}/accesses")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
[AuthorizeMinUserType(UserTypes.ADMIN)]
public class UserAccessesController : ControllerBase
{
    /// <summary>
    /// The user group access service
    /// </summary>
    private readonly UserAccessService userAccessService;

    /// <summary>
    /// Creates new instance of user access controller
    /// </summary>
    /// <param name="userAccessService">The user access service</param>
    public UserAccessesController(UserAccessService userAccessService)
    {
        this.userAccessService = userAccessService;
    }
    
    /// <summary>
    /// Gets the accesses assigned to user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_USERS_READ)]
    [HttpGet]
    public Task<IEnumerable<UserAccessModel>> Get(string userId)
    {
        return this.userAccessService.Get(userId);
    }

    /// <summary>
    /// Updates the accesses assigned to user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="input">The input to update</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_USERS_MANAGE_ACCESS)]
    [HttpPost]
    [HttpPut]
    public Task<UserAccessUpdateResultModel> Update(string userId, UserAccessUpdateModel input)
    {
        return this.userAccessService.Update(userId, input);
    }
}