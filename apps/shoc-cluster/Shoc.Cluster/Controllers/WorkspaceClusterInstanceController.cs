using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Cluster.Model.Cluster;
using Shoc.Cluster.Services;

namespace Shoc.Cluster.Controllers;

/// <summary>
/// The clusters details endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/workspace-clusters/{id}/instance")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class WorkspaceClusterDetailsController : ControllerBase
{
    /// <summary>
    /// The cluster details service
    /// </summary>
    private readonly WorkspaceClusterInstanceService clusterInstanceService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="clusterInstanceService">The cluster details service</param>
    public WorkspaceClusterDetailsController(WorkspaceClusterInstanceService clusterInstanceService)
    {
        this.clusterInstanceService = clusterInstanceService;
    }

    /// <summary>
    /// Gets the cluster connectivity by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the cluster</param>
    /// <returns></returns>
    [HttpGet("connectivity")]
    public Task<ClusterConnectivityModel> GetConnectivityById(string workspaceId, string id)
    {
        return this.clusterInstanceService.GetConnectivityById(this.HttpContext.GetPrincipal().Id, workspaceId, id);
    }
    
    /// <summary>
    /// Gets the cluster nodes by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the cluster</param>
    /// <returns></returns>
    [HttpGet("nodes")]
    public Task<IEnumerable<ClusterNodeModel>> GetNodesById(string workspaceId, string id)
    {
        return this.clusterInstanceService.GetNodesById(this.HttpContext.GetPrincipal().Id, workspaceId, id);
    }
}

