using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Identity.Model.Roles;
using Shoc.Identity.Model.User;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The role member repository.
/// </summary>
public class RoleMemberRepository : IRoleMemberRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of role user members repository implementation.
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public RoleMemberRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the users in the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <returns>A task containing all role users.</returns>
    public Task<IEnumerable<UserReferentialValueModel>> GetAll(string roleId)
    {
        return this.dataOps.Connect().Query("Identity.Role.Member", "GetAll").ExecuteAsync<UserReferentialValueModel>(new
        {
            RoleId = roleId
        });
    }

    /// <summary>
    /// Gets the particular user in the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the role use.</returns>
    public Task<UserReferentialValueModel> GetById(string roleId, string userId)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Role.Member", "GetById").ExecuteAsync<UserReferentialValueModel>(new
        {
            RoleId = roleId,
            UserId = userId
        });
    }

    /// <summary>
    /// Creates new role user membership record by the input.
    /// </summary>
    /// <param name="input">The role user membership input.</param>
    /// <returns>A task containing the created role user membership.</returns>
    public Task<UserReferentialValueModel> Create(RoleMembership input)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Role.Member", "Create").ExecuteAsync<UserReferentialValueModel>(input);
    }

    /// <summary>
    /// Deletes the member user from the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the deleted user from the role.</returns>
    public Task<UserReferentialValueModel> DeleteById(string roleId, string userId)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Role.Member", "DeleteById").ExecuteAsync<UserReferentialValueModel>(new
        {
            RoleId = roleId,
            UserId = userId
        });
    }
}
