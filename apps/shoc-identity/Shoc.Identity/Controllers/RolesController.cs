using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Roles;
using Shoc.Identity.Services;

namespace Shoc.Identity.Controllers;

/// <summary>
/// The roles controller.
/// </summary>
[Route("api/roles")]
[ApiController]
[ShocExceptionHandler]
[BearerOnly]
public class RolesController : ControllerBase
{
    /// <summary>
    /// The role service.
    /// </summary>
    private readonly RoleService roleService;

    /// <summary>
    /// Creates a new instance of the roles controller.
    /// </summary>
    /// <param name="roleService">The role service.</param>
    public RolesController(RoleService roleService)
    {
        this.roleService = roleService;
    }

    /// <summary>
    /// Get all roles.
    /// </summary>
    /// <returns>A task containing all roles.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_LIST)]
    [HttpGet]
    public Task<IEnumerable<RoleModel>> GetAll()
    {
        return this.roleService.GetAll();
    }

    /// <summary>
    /// Get all privilege referential values.
    /// </summary>
    /// <returns>A task containing all privilege references.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_LIST_REFERENCES)]
    [HttpGet("referential-values")]
    public async Task<IEnumerable<RoleReferentialValueModel>> GetAllReferentialValues()
    {
        return await this.roleService.GetAllReferentialValues();
    }

    /// <summary>
    /// Get the role by id.
    /// </summary>
    /// <param name="id">The role id.</param>
    /// <returns>A task containing the role.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_READ)]
    [HttpGet("{id}")]
    public Task<RoleModel> GetById(string id)
    {
        return this.roleService.GetById(id);
    }

    /// <summary>
    /// Creates role by given input.
    /// </summary>
    /// <param name="input">The role creation input.</param>
    /// <returns>A task containing the newly created role.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_CREATE)]
    [HttpPost]
    public Task<RoleModel> Create([FromBody] RoleCreateModel input)
    {
        return this.roleService.Create(input);
    }

    /// <summary>
    /// Updates the role by given input.
    /// </summary>
    /// <param name="id">The role id.</param>
    /// <param name="input">The role update input.</param>
    /// <returns>A task containing the updated role.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_EDIT)]
    [HttpPut("{id}")]
    public Task<RoleModel> UpdateById(string id, RoleUpdateModel input)
    {
        return this.roleService.UpdateById(id, input);
    }

    /// <summary>
    /// Deletes the role by specified id.
    /// </summary>
    /// <param name="id">The role id.</param>
    /// <returns>A task containing the deleted role.</returns>
    [AuthorizeAnyAccess(IdentityAccesses.CONNECT_ROLES_DELETE)]
    [HttpDelete("{id}")]
    public Task<RoleModel> DeleteById(string id)
    {
        return this.roleService.DeleteById(id);
    }
}
