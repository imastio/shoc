using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Registry.Model.Key;

namespace Shoc.Registry.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IRegistrySigningKeyRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    Task<IEnumerable<RegistrySigningKeyModel>> GetAll(string registryId);

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    Task<RegistrySigningKeyModel> GetById(string registryId, string id);
    
    /// <summary>
    /// Gets the object by key id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="keyId">The key id</param>
    /// <returns></returns>
    Task<RegistrySigningKeyModel> GetByKeyId(string registryId, string keyId);
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<RegistrySigningKeyModel> Create(RegistrySigningKeyCreateModel input);

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<RegistrySigningKeyModel> DeleteById(string registryId, string id);
}