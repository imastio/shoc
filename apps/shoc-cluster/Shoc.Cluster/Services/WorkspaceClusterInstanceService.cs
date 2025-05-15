using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Cluster.Model.Cluster;
using Shoc.ObjectAccess.Cluster;
using Shoc.ObjectAccess.Model.Cluster;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Cluster.Services;

/// <summary>
/// The workspace cluster service
/// </summary>
public class WorkspaceClusterInstanceService : WorkspaceClusterServiceBase
{
    /// <summary>
    /// The cluster instance service
    /// </summary>
    private readonly ClusterInstanceService clusterInstanceService;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="clusterService">The cluster service</param>
    /// <param name="clusterInstanceService">The cluster instance service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    /// <param name="clusterAccessEvaluator">The cluster access evaluator</param>
    public WorkspaceClusterInstanceService(ClusterService clusterService, ClusterInstanceService clusterInstanceService, IWorkspaceAccessEvaluator workspaceAccessEvaluator, IClusterAccessEvaluator clusterAccessEvaluator) : base(clusterService, workspaceAccessEvaluator, clusterAccessEvaluator)
    {
        this.clusterInstanceService = clusterInstanceService;
    }
    
    /// <summary>
    /// Perform a dry test of configuration for a new object
    /// </summary>
    /// <returns></returns>
    public async Task<ClusterConnectionTestedModel> Ping(string userId, string workspaceId, ClusterConnectionTestModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_CLUSTERS);

        // perform the test and return the result
        return await this.clusterInstanceService.Ping(input);
    }

    /// <summary>
    /// Gets the connectivity info by id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ClusterConnectivityModel> GetConnectivityById(string userId, string workspaceId, string id)
    {
        // ensure have required workspace access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_CLUSTERS);
        
        // ensure have required cluster access
        await this.clusterAccessEvaluator.Ensure(
            userId,
            workspaceId,
            id,
            ClusterPermissions.CLUSTER_VIEW);

        // return the result
        return await this.clusterInstanceService.GetConnectivityById(workspaceId, id);
    }

    /// <summary>
    /// Gets the connectivity info by id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ClusterNodeModel>> GetNodesById(string userId, string workspaceId, string id)
    {
        // ensure have required workspace access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_CLUSTERS);
        
        // ensure have required cluster access
        await this.clusterAccessEvaluator.Ensure(
            userId,
            workspaceId,
            id,
            ClusterPermissions.CLUSTER_VIEW);

        // return the result
        return await this.clusterInstanceService.GetNodesById(workspaceId, id);
    }
}