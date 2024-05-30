using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.User;
using Shoc.Identity.Model.UserGroup;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The user group member repository
/// </summary>
public interface IUserGroupMemberRepository
{
    /// <summary>
    /// Gets all the users in the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <returns></returns>
    Task<IEnumerable<UserReferentialValueModel>> GetAll(string groupId);

    /// <summary>
    /// Gets the particular user in the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    Task<UserReferentialValueModel> GetById(string groupId, string userId);

    /// <summary>
    /// Creates new membership record for the given
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<UserReferentialValueModel> Create(UserGroupMembership input);

    /// <summary>
    /// Deletes the member user from the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    Task<UserReferentialValueModel> DeleteById(string groupId, string userId);
}