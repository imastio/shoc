using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Registry.Model.Credential;

namespace Shoc.Registry.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IRegistryCredentialRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    Task<IEnumerable<RegistryCredentialModel>> GetAll(string registryId);

    /// <summary>
    /// Gets all the objects with filter
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="filter">The filter to apply</param>
    /// <returns></returns>
    Task<IEnumerable<RegistryCredentialModel>> GetBy(string registryId, RegistryCredentialFilter filter);
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    Task<IEnumerable<RegistryCredentialExtendedModel>> GetAllExtended(string registryId);
    
    /// <summary>
    /// Gets all the extended objects by filter
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="filter">The filter to apply</param>
    /// <returns></returns>
    Task<IEnumerable<RegistryCredentialExtendedModel>> GetExtendedBy(string registryId, RegistryCredentialFilter filter);
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    Task<RegistryCredentialModel> GetById(string registryId, string id);
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<RegistryCredentialModel> Create(RegistryCredentialCreateModel input);

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<RegistryCredentialModel> UpdateById(string registryId, string id, RegistryCredentialUpdateModel input);

    /// <summary>
    /// Updates the object password by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<RegistryCredentialModel> UpdatePasswordById(string registryId, string id, RegistryCredentialPasswordUpdateModel input);

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<RegistryCredentialModel> DeleteById(string registryId, string id);
}