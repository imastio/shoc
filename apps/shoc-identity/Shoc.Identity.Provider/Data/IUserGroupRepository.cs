using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.UserGroup;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The user groups repository
/// </summary>
public interface IUserGroupRepository
{
    /// <summary>
    /// Gets all the user groups
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<UserGroupModel>> GetAll();

    /// <summary>
    /// Gets the user group by id
    /// </summary>
    /// <param name="id">The user group id</param>
    /// <returns></returns>
    Task<UserGroupModel> GetById(string id);
    
    /// <summary>
    /// Gets the user group by name
    /// </summary>
    /// <param name="name">The user group name</param>
    /// <returns></returns>
    Task<UserGroupModel> GetByName(string name);

    /// <summary>
    /// Creates entity based on input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<UserGroupModel> Create(UserGroupCreateModel input);

    /// <summary>
    /// Updates the user group by given input.
    /// </summary>
    /// <param name="input">The user group input.</param>
    /// <returns></returns>
    Task<UserGroupModel> UpdateById(UserGroupUpdateModel input);

    /// <summary>
    /// Deletes the user group by id
    /// </summary>
    /// <param name="id">The user group id</param>
    /// <returns></returns>
    Task<UserGroupModel> DeleteById(string id);
}
