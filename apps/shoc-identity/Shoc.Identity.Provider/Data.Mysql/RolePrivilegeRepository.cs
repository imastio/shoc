using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Identity.Model.Privileges;
using Shoc.Identity.Model.Roles;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The role privilege repository.
/// </summary>
public class RolePrivilegeRepository : IRolePrivilegeRepository
{
    /// <summary>
    /// The data operations instance.
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of role privileges repository implementation.
    /// </summary>
    /// <param name="dataOps">A DataOps instance.</param>
    public RolePrivilegeRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Get all role privileges.
    /// </summary>
    /// <param name="roleId">The role identifier.</param>
    /// <returns>A task containing all privileges of specified role.</returns>
    public Task<IEnumerable<PrivilegeReferentialValueModel>> GetAll(string roleId)
    {
        return this.dataOps.Connect().Query("Identity.Role.Privilege", "GetAll").ExecuteAsync<PrivilegeReferentialValueModel>(new
        {
            RoleId = roleId,
        });
    }

    /// <summary>
    /// Add privilege to specified role.
    /// </summary>
    /// <param name="input">The role privilege creation input.</param>
    /// <returns>A task containing the added role privilege.</returns>
    public Task<PrivilegeReferentialValueModel> Create(RolePrivilegeCreateModel input)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Role.Privilege", "Create").ExecuteAsync<PrivilegeReferentialValueModel>(input);
    }

    /// <summary>
    /// Remove privilege from the specified role.
    /// </summary>
    /// <param name="privilegeId">The privilege identifier.</param>
    /// <param name="roleId">The role identifier.</param>
    /// <returns>A task containing the removed role privilege.</returns>
    public Task<PrivilegeReferentialValueModel> DeleteById(string privilegeId, string roleId)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Role.Privilege", "DeleteById").ExecuteAsync<PrivilegeReferentialValueModel>(new
        {
            RoleId = roleId,
            PrivilegeId = privilegeId
        });
    }
}
