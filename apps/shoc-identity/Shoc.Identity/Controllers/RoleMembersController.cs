using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Roles;
using Shoc.Identity.Model.User;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The role members controller.
/// </summary>
[Route("api/roles/{roleId}/members")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class RoleMembersController : ControllerBase
{
    /// <summary>
    /// The role members service.
    /// </summary>
    private readonly RoleMembersService roleMembersService;

    /// <summary>
    /// Creates new instance of role members controller.
    /// </summary>
    /// <param name="roleMembersService">The role members service.</param>
    public RoleMembersController(RoleMembersService roleMembersService)
    {
        this.roleMembersService = roleMembersService;
    }

    /// <summary>
    /// Gets all the member users by the role id.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <returns>A task containing the users of the specified role.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_READ)]
    [HttpGet]
    public Task<IEnumerable<UserReferentialValueModel>> GetAll(string roleId)
    {
        return this.roleMembersService.GetAll(roleId);
    }

    /// <summary>
    /// Gets the particular user in the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the role user.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_READ)]
    [HttpGet("{userId}")]
    public Task<UserReferentialValueModel> GetById(string roleId, string userId)
    {
        return this.roleMembersService.GetById(roleId, userId);
    }

    /// <summary>
    /// Creates new role user membership record by the input.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="input">The role user membership input.</param>
    /// <returns>A task containing the created role user membership.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_MANAGE)]
    [HttpPost]
    public Task<UserReferentialValueModel> Create(string roleId, RoleMembership input)
    {
        return this.roleMembersService.Create(roleId, input);
    }

    /// <summary>
    /// Deletes the member user from the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the deleted user from the role.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_MANAGE)]
    [HttpDelete("{userId}")]
    public Task<UserReferentialValueModel> DeleteById(string roleId, string userId)
    {
        return this.roleMembersService.DeleteById(roleId, userId);
    }
}
