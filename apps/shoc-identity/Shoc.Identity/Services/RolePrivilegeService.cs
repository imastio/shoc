using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model.Privileges;
using Shoc.Identity.Model.Roles;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The role privilege service.
/// </summary>
public class RolePrivilegeService : RoleServiceBase
{
    /// <summary>
    /// The role privilege repository.
    /// </summary>
    private readonly IRolePrivilegeRepository rolePrivilegeRepository;

    /// <summary>
    /// The privilege repository.
    /// </summary>
    private readonly IPrivilegeRepository privilegeRepository;

    /// <summary>
    /// Creates new instance of role privilege service.
    /// </summary>
    /// <param name="rolePrivilegeRepository">The role privilege repository.</param>
    /// <param name="roleRepository">The role repository</param>
    /// <param name="privilegeRepository">The privilege repository</param>
    public RolePrivilegeService(IRolePrivilegeRepository rolePrivilegeRepository, IRoleRepository roleRepository, IPrivilegeRepository privilegeRepository) : base(roleRepository)
    {
        this.rolePrivilegeRepository = rolePrivilegeRepository;
        this.privilegeRepository = privilegeRepository;
    }

    /// <summary>
    /// Gets all roles.
    /// </summary>
    /// <param name="roleId">The role identifier.</param>
    /// <returns>A task containing the roles.</returns>
    public async Task<IEnumerable<PrivilegeReferentialValueModel>> GetAll(string roleId)
    {
        // make sure the role exists
        await this.RequireById(roleId);

        // get the role privileges
        return await this.rolePrivilegeRepository.GetAll(roleId);
    }

    /// <summary>
    /// Add privilege to the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="input">The role privilege creation input.</param>
    /// <returns>A task containing the added role privilege.</returns>
    public async Task<PrivilegeReferentialValueModel> Create(string roleId, RolePrivilegeCreateModel input)
    {
        // make sure referring to same object
        input.RoleId = roleId;

        // make sure the role and privilege exist
        await Task.WhenAll(this.RequireById(roleId), this.RequirePrivilegeById(input.PrivilegeId));

        // add privilege to role
        return await this.rolePrivilegeRepository.Create(input);
    }

    /// <summary>
    /// Remove the privilege from the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="privilegeId">The privilege id.</param>
    /// <returns></returns>
    public async Task<PrivilegeReferentialValueModel> DeleteById(string roleId, string privilegeId)
    {
        // make sure the role and privilege exist
        await Task.WhenAll(this.RequireById(roleId), this.RequirePrivilegeById(privilegeId));

        // delete the role privilege
        return await this.rolePrivilegeRepository.DeleteById(privilegeId, roleId);
    }

    /// <summary>
    /// Require privilege by given id.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <returns>A task containing the privilege model.</returns>
    private async Task<PrivilegeModel> RequirePrivilegeById(string privilegeId)
    {
        // get privilege by id
        var result = await this.privilegeRepository.GetById(privilegeId);

        // handle if not found
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        return result;
    }
}
