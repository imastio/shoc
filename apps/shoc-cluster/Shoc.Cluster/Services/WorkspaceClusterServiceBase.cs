using System.Threading.Tasks;
using Shoc.Cluster.Model.Cluster;
using Shoc.Cluster.Model.WorkspaceCluster;
using Shoc.ObjectAccess.Cluster;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Cluster.Services;

/// <summary>
/// The workspace cluster service base
/// </summary>
public abstract class WorkspaceClusterServiceBase
{
    /// <summary>
    /// The cluster service
    /// </summary>
    protected readonly ClusterService clusterService;

    /// <summary>
    /// The workspace access evaluator
    /// </summary>
    protected readonly IWorkspaceAccessEvaluator workspaceAccessEvaluator;

    /// <summary>
    /// The cluster access evaluator
    /// </summary>
    protected readonly IClusterAccessEvaluator clusterAccessEvaluator;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="clusterService">The cluster service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    /// <param name="clusterAccessEvaluator">The cluster access evaluator</param>
    protected WorkspaceClusterServiceBase(ClusterService clusterService, IWorkspaceAccessEvaluator workspaceAccessEvaluator, IClusterAccessEvaluator clusterAccessEvaluator)
    {
        this.clusterService = clusterService;
        this.workspaceAccessEvaluator = workspaceAccessEvaluator;
        this.clusterAccessEvaluator = clusterAccessEvaluator;
    }
    
    /// <summary>
    /// Requires the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    protected async Task<WorkspaceClusterModel> RequireById(string workspaceId, string id)
    {
        // get the result
        var result = await this.clusterService.GetExtendedById(workspaceId, id);

        return Map(result);
    }
    
    /// <summary>
    /// Requires the object by name
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="name">The name of the object</param>
    /// <returns></returns>
    protected async Task<WorkspaceClusterModel> RequireByName(string workspaceId, string name)
    {
        // get the result
        var result = await this.clusterService.GetExtendedByName(workspaceId, name);

        return Map(result);
    }

    /// <summary>
    /// Maps the cluster model into a workspace cluster model
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    protected static WorkspaceClusterModel Map(ClusterExtendedModel input)
    {
        return new WorkspaceClusterModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            WorkspaceName = input.WorkspaceName,
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Status = input.Status,
            Created = input.Created,
            Updated = input.Updated
        };
    }
}