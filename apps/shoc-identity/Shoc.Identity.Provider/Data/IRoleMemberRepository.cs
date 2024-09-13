using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Roles;
using Shoc.Identity.Model.User;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The role member repository interface.
/// </summary>
public interface IRoleMemberRepository
{
    /// <summary>
    /// Gets all the users in the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <returns>A task containing all role users.</returns>
    Task<IEnumerable<UserReferentialValueModel>> GetAll(string roleId);

    /// <summary>
    /// Gets the particular user in the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the role use.</returns>
    Task<UserReferentialValueModel> GetById(string roleId, string userId);

    /// <summary>
    /// Creates new role user membership record for the given input.
    /// </summary>
    /// <param name="input">The role user membership input.</param>
    /// <returns>A task containing the created role user membership.</returns>
    Task<UserReferentialValueModel> Create(RoleMembership input);

    /// <summary>
    /// Deletes the member user from the role.
    /// </summary>
    /// <param name="roleId">The role id.</param>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the deleted user from the role.</returns>
    Task<UserReferentialValueModel> DeleteById(string roleId, string userId);
}
