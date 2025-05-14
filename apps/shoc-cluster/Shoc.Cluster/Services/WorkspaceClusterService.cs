using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Cluster.Model.Cluster;
using Shoc.Cluster.Model.WorkspaceCluster;
using Shoc.ObjectAccess.Cluster;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Cluster.Services;

/// <summary>
/// The workspace cluster service
/// </summary>
public class WorkspaceClusterService : WorkspaceClusterServiceBase
{
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="clusterService">The cluster service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    /// <param name="clusterAccessEvaluator">The cluster access evaluator</param>
    public WorkspaceClusterService(ClusterService clusterService, IWorkspaceAccessEvaluator workspaceAccessEvaluator, IClusterAccessEvaluator clusterAccessEvaluator) : base(clusterService, workspaceAccessEvaluator, clusterAccessEvaluator)
    {
    }
    
    /// <summary>
    /// Gets all objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<WorkspaceClusterModel>> GetAll(string userId, string workspaceId)
    {
        // load items
        var items = await this.clusterService.GetAllExtended(workspaceId);
        
        // ensure we have a permission to view workspace clusters
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_CLUSTERS);

        // map and return the result
        return items.Select(Map);
    }
    
    /// <summary>
    /// Gets object by name
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceClusterModel> GetByName(string userId, string workspaceId, string name)
    {
        // ensure we have a permission to view workspace clusters
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_CLUSTERS);

        // gets the item by name
        var item = await this.clusterService.GetExtendedByName(workspaceId, name);
        
        // map and return the result
        return Map(item);
    }

    /// <summary>
    /// Gets object permissions by name
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="name">The name of object</param>
    /// <returns></returns>
    public async Task<ISet<string>> GetPermissionsByName(string userId, string workspaceId, string name)
    {
        // make sure object exists
        var result = await this.GetByName(userId, workspaceId, name);
        
        return await this.clusterAccessEvaluator.GetPermissions(userId, result.WorkspaceId, result.Id);
    }
    
    /// <summary>
    /// Counts all objects
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceClusterCountModel> CountAll(string userId, string workspaceId)
    {
        // count the objects
        var count = await this.clusterService.CountAll(workspaceId);
        
        // ensure we have a permission to view workspace clusters
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_CLUSTERS);

        // map and return the result
        return new WorkspaceClusterCountModel
        {
            Count = count.Count
        };
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceClusterCreatedModel> Create(string userId, string workspaceId, WorkspaceClusterCreateModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_CLUSTERS,
            WorkspacePermissions.WORKSPACE_CREATE_CLUSTER);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // create the object
        var result = await this.clusterService.Create(workspaceId, new ClusterCreateModel
        {
            WorkspaceId = input.WorkspaceId,
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Configuration = input.Configuration
        });
        
        // return existing object
        return new WorkspaceClusterCreatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }
}