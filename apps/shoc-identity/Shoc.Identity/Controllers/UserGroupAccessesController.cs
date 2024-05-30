using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Identity.Model;
using Shoc.Identity.Model.User;
using Shoc.Identity.Model.UserGroup;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The user groups accesses controller
/// </summary>
[Route("api/user-groups/{groupId}/accesses")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
[AuthorizeMinUserType(UserTypes.ADMIN)]
public class UserGroupAccessesController : ControllerBase
{
    /// <summary>
    /// The user group access service
    /// </summary>
    private readonly UserGroupAccessService userGroupAccessService;

    /// <summary>
    /// Creates new instance of user group accesses controller
    /// </summary>
    /// <param name="userGroupAccessService">The user group access service</param>
    public UserGroupAccessesController(UserGroupAccessService userGroupAccessService)
    {
        this.userGroupAccessService = userGroupAccessService;
    }
    
    /// <summary>
    /// Gets the accesses assigned to user group
    /// </summary>
    /// <param name="groupId">The user group id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USER_GROUPS_READ)]
    [HttpGet]
    public Task<IEnumerable<UserGroupAccessModel>> Get(string groupId)
    {
        return this.userGroupAccessService.Get(groupId);
    }
    
    /// <summary>
    /// Updates the accesses assigned to user group
    /// </summary>
    /// <param name="groupId">The user group id</param>
    /// <param name="input">The input to update</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_USER_GROUPS_MANAGE_ACCESS)]
    [HttpPost]
    [HttpPut]
    public Task<UserGroupAccessUpdateResultModel> Update(string groupId, UserGroupAccessUpdateModel input)
    {
        return this.userGroupAccessService.Update(groupId, input);
    }
}