using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.User;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The user access repository interface.
/// </summary>
public interface IUserAccessRepository
{
    /// <summary>
    /// Gets the all user access definitions.
    /// </summary>
    /// <returns>A task containing all user accesses.</returns>
    Task<IEnumerable<UserAccessModel>> GetAll();

    /// <summary>
    /// Gets user access definition by id.
    /// </summary>
    /// <param name="id">The id of definition.</param>
    /// <returns>A task containing the user access definition.</returns>

    Task<UserAccessModel> GetById(string id);

    /// <summary>
    /// Gets all user access definition by user id.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the user access definitions.</returns>

    Task<IEnumerable<UserAccessModel>> GetByUserId(string userId);

    /// <summary>
    /// Update user access based on the input.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="input">The access model update input.</param>
    /// <returns>A task containing the user access update result.</returns>
    Task<UserAccessUpdateResultModel> UpdateByUserId(string userId, UserAccessUpdateModel input);

    /// <summary>
    /// Deletes user access grant by identifier.
    /// </summary>
    /// <param name="id">The user access declaration id</param>
    /// <returns>A task containing the deleted user acess definition.</returns>
    Task<UserAccessModel> DeleteById(string id);
}
