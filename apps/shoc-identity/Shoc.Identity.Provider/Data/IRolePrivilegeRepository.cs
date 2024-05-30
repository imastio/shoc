using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Privileges;
using Shoc.Identity.Model.Roles;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The role privilege repository.
/// </summary>
public interface IRolePrivilegeRepository
{
    /// <summary>
    /// Get all privileges of the specified role.
    /// </summary>
    /// <returns>A task containing all privileges of the specified role.</returns>
    Task<IEnumerable<PrivilegeReferentialValueModel>> GetAll(string roleId);

    /// <summary>
    /// Add privilege to specified role.
    /// </summary>
    /// <param name="input">The role privilege creation input.</param>
    /// <returns>A task containing newly added role privilege.</returns>
    Task<PrivilegeReferentialValueModel> Create(RolePrivilegeCreateModel input);

    /// <summary>
    /// Deletes the role by id.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <param name="roleId">The role id.</param>
    /// <returns>A task containing the deleted role privilege.</returns>
    Task<PrivilegeReferentialValueModel> DeleteById(string privilegeId, string roleId);
}
