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
/// The user groups members controller
/// </summary>
[Route("api/user-groups/{groupId}/members")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class UserGroupMembersController : ControllerBase
{
    /// <summary>
    /// The user group members service
    /// </summary>
    private readonly UserGroupMembersService userGroupMembersService;

    /// <summary>
    /// Creates new instance of user groups controller
    /// </summary>
    /// <param name="userGroupMembersService">The user group members service</param>
    public UserGroupMembersController(UserGroupMembersService userGroupMembersService)
    {
        this.userGroupMembersService = userGroupMembersService;
    }

    /// <summary>
    /// Gets all user groups members
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_USER_GROUPS_READ)]
    [HttpGet]
    public Task<IEnumerable<UserReferentialValueModel>> GetAll(string groupId)
    {
        return this.userGroupMembersService.GetAll(groupId);
    }

    /// <summary>
    /// Gets the particular user in the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_USER_GROUPS_READ)]
    [HttpGet("{userId}")]
    public Task<UserReferentialValueModel> GetById(string groupId, string userId)
    {
        return this.userGroupMembersService.GetById(groupId, userId);
    }

    /// <summary>
    /// Creates new membership record for the given
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="input"></param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_USER_GROUPS_MANAGE)]
    [HttpPost]
    public Task<UserReferentialValueModel> Create(string groupId, UserGroupMembership input)
    {
        return this.userGroupMembersService.Create(groupId, input);
    }
    
    /// <summary>
    /// Deletes the member user from the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_USER_GROUPS_MANAGE)]
    [HttpDelete("{userId}")]
    public Task<UserReferentialValueModel> DeleteById(string groupId, string userId)
    {
        return this.userGroupMembersService.DeleteById(groupId, userId);
    }
}
