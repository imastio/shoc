using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Cluster.Model;
using Shoc.Cluster.Model.Cluster;
using Shoc.Cluster.Services;

namespace Shoc.Cluster.Controllers;

/// <summary>
/// The clusters endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/clusters")]
[ApiController]
[ShocExceptionHandler]
public class ClustersController : ControllerBase
{
    /// <summary>
    /// The cluster service
    /// </summary>
    private readonly ClusterService clusterService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="clusterService">The reference to service</param>
    public ClustersController(ClusterService clusterService)
    {
        this.clusterService = clusterService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_LIST)]
    [HttpGet]
    public Task<IEnumerable<ClusterModel>> GetAll(string workspaceId)
    {
        return this.clusterService.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_LIST)]
    [HttpGet("extended")]
    public Task<IEnumerable<ClusterExtendedModel>> GetAllExtended(string workspaceId)
    {
        return this.clusterService.GetAllExtended(workspaceId);
    }

    /// <summary>
    /// Get all referential values
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_LIST_REFERENCES)]
    [HttpGet("referential-values")]
    public Task<IEnumerable<ClusterReferentialValueModel>> GetAllReferentialValues(string workspaceId)
    {
        return this.clusterService.GetAllReferentialValues(workspaceId);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_READ)]
    [HttpGet("{id}")]
    public Task<ClusterModel> GetById(string workspaceId, string id)
    {
        return this.clusterService.GetById(workspaceId, id);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_READ)]
    [HttpGet("{id}/extended")]
    public Task<ClusterExtendedModel> GetExtendedById(string workspaceId, string id)
    {
        return this.clusterService.GetExtendedById(workspaceId, id);
    }
    
    /// <summary>
    /// Creates the new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The create input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_CREATE)]
    [HttpPost]
    public Task<ClusterModel> Create(string workspaceId, ClusterCreateModel input)
    {
        return this.clusterService.Create(workspaceId, input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_EDIT)]
    [HttpPut("{id}")]
    public Task<ClusterModel> UpdateById(string workspaceId, string id, ClusterUpdateModel input)
    {
        return this.clusterService.UpdateById(workspaceId, id, input);
    }
    
    /// <summary>
    /// Updates the object configuration by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_MANAGE)]
    [HttpPut("{id}/configuration")]
    public Task<ClusterModel> UpdateConfigurationById(string workspaceId, string id, ClusterConfigurationUpdateModel input)
    {
        return this.clusterService.UpdateConfigurationById(workspaceId, id, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_DELETE)]
    [HttpDelete("{id}")]
    public Task<ClusterModel> DeleteById(string workspaceId, string id)
    {
        return this.clusterService.DeleteById(workspaceId, id);
    }
}

