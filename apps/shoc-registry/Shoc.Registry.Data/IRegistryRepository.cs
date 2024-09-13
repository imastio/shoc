using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Registry.Model.Registry;

namespace Shoc.Registry.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IRegistryRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<RegistryModel>> GetAll();
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<RegistryExtendedModel>> GetAllExtended();
    
    /// <summary>
    /// Get all referential values
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<RegistryReferentialValueModel>> GetAllReferentialValues();
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<RegistryModel> GetById(string id);
    
    /// <summary>
    /// Gets the object by workspace id and the name 
    /// </summary>
    /// <returns></returns>
    Task<RegistryModel> GetByName(string workspaceId, string name);

    /// <summary>
    /// Gets the object by global name 
    /// </summary>
    /// <returns></returns>
    Task<RegistryModel> GetByGlobalName(string name);
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<RegistryModel> Create(RegistryCreateModel input);

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<RegistryModel> UpdateById(string id, RegistryUpdateModel input);

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<RegistryModel> DeleteById(string id);
}