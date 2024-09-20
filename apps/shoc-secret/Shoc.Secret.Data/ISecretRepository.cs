using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Secret.Model.Secret;

namespace Shoc.Secret.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface ISecretRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SecretModel>> GetAll(string workspaceId);
    
    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SecretExtendedModel>> GetAllExtended(string workspaceId);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<SecretModel> GetById(string workspaceId, string id);
   
    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <returns></returns>
    Task<SecretModel> GetByName(string workspaceId, string name);
    
    /// <summary>
    /// Count objects by workspace id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    Task<SecretCountModel> CountAll(string workspaceId);

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<SecretModel> Create(string workspaceId, SecretCreateModel input);

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<SecretModel> UpdateById(string workspaceId, string id, SecretUpdateModel input);

    /// <summary>
    /// Updates the object value by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<SecretModel> UpdateValueById(string workspaceId, string id, SecretValueUpdateModel input);

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<SecretModel> DeleteById(string workspaceId, string id);
}