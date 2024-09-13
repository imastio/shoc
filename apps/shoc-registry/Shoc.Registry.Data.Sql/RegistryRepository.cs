using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Registry;

namespace Shoc.Registry.Data.Sql;

/// <summary>
/// The repository implementation
/// </summary>
public class RegistryRepository : IRegistryRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public RegistryRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<RegistryModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Registry.Registry", "GetAll").ExecuteAsync<RegistryModel>();
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<RegistryExtendedModel>> GetAllExtended()
    {
        return this.dataOps.Connect().Query("Registry.Registry", "GetAllExtended").ExecuteAsync<RegistryExtendedModel>();
    }

    /// <summary>
    /// Get all referential values
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<RegistryReferentialValueModel>> GetAllReferentialValues()
    {
        return this.dataOps.Connect().Query("Registry.Registry", "GetAllReferentialValues").ExecuteAsync<RegistryReferentialValueModel>();
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<RegistryModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Registry.Registry", "GetById").ExecuteAsync<RegistryModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Gets the object by workspace id and the name 
    /// </summary>
    /// <returns></returns>
    public Task<RegistryModel> GetByName(string workspaceId, string name)
    {
        return this.dataOps.Connect().QueryFirst("Registry.Registry", "GetByName").ExecuteAsync<RegistryModel>(new
        {
            WorkspaceId = workspaceId,
            Name = name
        });
    }
    
    /// <summary>
    /// Gets the object by global name 
    /// </summary>
    /// <returns></returns>
    public Task<RegistryModel> GetByGlobalName(string name)
    {
        return this.dataOps.Connect().QueryFirst("Registry.Registry", "GetByGlobalName").ExecuteAsync<RegistryModel>(new
        {
            Name = name
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<RegistryModel> Create(RegistryCreateModel input)
    {
        // assign the id
        input.Id ??= StdIdGenerator.Next(RegistryObjects.REGISTRY).ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Registry.Registry", "Create").ExecuteAsync<RegistryModel>(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<RegistryModel> UpdateById(string id, RegistryUpdateModel input)
    {
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Registry.Registry", "UpdateById").ExecuteAsync<RegistryModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<RegistryModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Registry.Registry", "DeleteById").ExecuteAsync<RegistryModel>(new
        {
            Id = id
        });    
    }
}