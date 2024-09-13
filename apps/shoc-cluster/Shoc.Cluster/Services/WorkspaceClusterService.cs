using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Cluster.Model.WorkspaceCluster;
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
    public WorkspaceClusterService(ClusterService clusterService, IWorkspaceAccessEvaluator workspaceAccessEvaluator) : base(clusterService, workspaceAccessEvaluator)
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
        await this.workspaceAccessEvaluator.Evaluate(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_CLUSTERS);

        // map and return the result
        return items.Select(Map);
    }
}