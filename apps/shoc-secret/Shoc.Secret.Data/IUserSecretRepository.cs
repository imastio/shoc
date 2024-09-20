using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Secret.Model.UserSecret;

namespace Shoc.Secret.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IUserSecretRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<UserSecretModel>> GetAll(string workspaceId, string userId);
    
    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<UserSecretExtendedModel>> GetAllExtended(string workspaceId, string userId);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<UserSecretModel> GetById(string workspaceId, string userId, string id);

    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <returns></returns>
    Task<UserSecretModel> GetByName(string workspaceId, string userId, string name);
    
    /// <summary>
    /// Count objects by workspace id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    Task<UserSecretCountModel> CountAll(string workspaceId, string userId);

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<UserSecretModel> Create(string workspaceId, string userId, UserSecretCreateModel input);

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<UserSecretModel> UpdateById(string workspaceId, string userId, string id, UserSecretUpdateModel input);

    /// <summary>
    /// Updates the object value by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<UserSecretModel> UpdateValueById(string workspaceId, string userId, string id, UserSecretValueUpdateModel input);

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<UserSecretModel> DeleteById(string workspaceId, string userId, string id);
}