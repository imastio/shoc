using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Cluster.Model;
using Shoc.Cluster.Model.Cluster;
using Shoc.Core;

namespace Shoc.Cluster.Data.Sql;

/// <summary>
/// The repository implementation
/// </summary>
public class ClusterRepository : IClusterRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public ClusterRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<ClusterModel>> GetAll(string workspaceId)
    {
        return this.dataOps.Connect().Query("Cluster", "GetAll")
            .WithBinding("ByWorkspace", !string.IsNullOrWhiteSpace(workspaceId))
            .ExecuteAsync<ClusterModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<ClusterExtendedModel>> GetAllExtended(string workspaceId)
    {
        return this.dataOps.Connect().Query("Cluster", "GetAllExtended")
            .WithBinding("ByWorkspace", !string.IsNullOrWhiteSpace(workspaceId))
            .ExecuteAsync<ClusterExtendedModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Get all referential values
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<ClusterReferentialValueModel>> GetAllReferentialValues(string workspaceId)
    {
        return this.dataOps.Connect().Query("Cluster", "GetAllReferentialValues").ExecuteAsync<ClusterReferentialValueModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<ClusterModel> GetById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Cluster", "GetById").ExecuteAsync<ClusterModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<ClusterExtendedModel> GetExtendedById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Cluster", "GetExtendedById").ExecuteAsync<ClusterExtendedModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
    
    /// <summary>
    /// Gets the object by workspace id and the name 
    /// </summary>
    /// <returns></returns>
    public Task<ClusterModel> GetByName(string workspaceId, string name)
    {
        return this.dataOps.Connect().QueryFirst("Cluster", "GetByName").ExecuteAsync<ClusterModel>(new
        {
            WorkspaceId = workspaceId,
            Name = name
        });
    }
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<ClusterModel> Create(string workspaceId, ClusterCreateModel input)
    {
        // assign the id
        input.WorkspaceId = workspaceId;
        input.Id ??= StdIdGenerator.Next(ClusterObjects.CLUSTER).ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Cluster", "Create").ExecuteAsync<ClusterModel>(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<ClusterModel> UpdateById(string workspaceId, string id, ClusterUpdateModel input)
    {
        input.WorkspaceId = workspaceId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Cluster", "UpdateById").ExecuteAsync<ClusterModel>(input);
    }
    
    /// <summary>
    /// Updates the object configuration by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<ClusterModel> UpdateConfigurationById(string workspaceId, string id, ClusterConfigurationUpdateModel input)
    {
        input.WorkspaceId = workspaceId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Cluster", "UpdateConfigurationById").ExecuteAsync<ClusterModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<ClusterModel> DeleteById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Cluster", "DeleteById").ExecuteAsync<ClusterModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });    
    }
}