using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Privileges;
using Shoc.Identity.Model.Roles;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The roles controller.
/// </summary>
[Route("api/roles/{roleId}/privileges")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class RolePrivilegesController : ControllerBase
{
    /// <summary>
    /// The role service.
    /// </summary>7
    private readonly RolePrivilegeService rolePrivilegeService;

    /// <summary>
    /// Creates a new instance of the role privileges controller.
    /// </summary>
    /// <param name="rolePrivilegeService">The role service.</param>
    public RolePrivilegesController(RolePrivilegeService rolePrivilegeService)
    {
        this.rolePrivilegeService = rolePrivilegeService;
    }

    /// <summary>
    /// Get privileges of the specified role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <returns>A task containing all privileges of the role.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_ROLES_READ)]
    [HttpGet]
    public Task<IEnumerable<PrivilegeReferentialValueModel>> GetAll(string roleId)
    {
        return this.rolePrivilegeService.GetAll(roleId);
    }

    /// <summary>
    /// Add privilege to the specified role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="input">The role privilege creation input.</param>
    /// <returns>A task containing an added role privilege.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_ROLES_MANAGE)]
    [HttpPost]
    public Task<PrivilegeReferentialValueModel> Create(string roleId, RolePrivilegeCreateModel input)
    {
        return this.rolePrivilegeService.Create(roleId, input);
    }

    /// <summary>
    /// Remove the privilege from the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="privilegeId">The privilege id.</param>
    /// <returns>A task containing an added role privilege.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.IDENTITY_ROLES_MANAGE)]
    [HttpDelete("{privilegeId}")]
    public Task<PrivilegeReferentialValueModel> DeleteById(string roleId, string privilegeId)
    {
        return this.rolePrivilegeService.DeleteById(roleId, privilegeId);
    }
}
