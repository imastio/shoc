using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.UserGroup;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The user group access repository.
/// </summary>
public interface IUserGroupAccessRepository
{
    /// <summary>
    /// Gets the all user group access definitions.
    /// </summary>
    /// <returns>A task containing all user group accesses.</returns>
    Task<IEnumerable<UserGroupAccessModel>> GetAll();

    /// <summary>
    /// Gets user group access definition by id.
    /// </summary>
    /// <param name="id">The id of definition.</param>
    /// <returns>A task containing the user group access definition.</returns>

    Task<UserGroupAccessModel> GetById(string id);

    /// <summary>
    /// Gets all user group access definition by user id.
    /// </summary>
    /// <param name="groupId">The group id.</param>
    /// <returns>A task containing the user group access definitions.</returns>

    Task<IEnumerable<UserGroupAccessModel>> GetByGroupId(string groupId);

    /// <summary>
    /// Update user group access based on the input.
    /// </summary>
    /// <param name="groupId">The group id.</param>
    /// <param name="input">The access model update input.</param>
    /// <returns>A task containing the user group access update result.</returns>
    Task<UserGroupAccessUpdateResultModel> UpdateByGroupId(string groupId, UserGroupAccessUpdateModel input);

    /// <summary>
    /// Deletes user group access grant by identifier.
    /// </summary>
    /// <param name="id">The user group access declaration id</param>
    /// <returns>A task containing the deleted user group acess definition.</returns>
    Task<UserGroupAccessModel> DeleteById(string id);
}
