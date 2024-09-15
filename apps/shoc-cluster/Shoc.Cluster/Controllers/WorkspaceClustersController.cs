using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Cluster.Model.WorkspaceCluster;
using Shoc.Cluster.Services;

namespace Shoc.Cluster.Controllers;

/// <summary>
/// The clusters endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/workspace-clusters")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class WorkspaceClustersController : ControllerBase
{
    /// <summary>
    /// The cluster service
    /// </summary>
    private readonly WorkspaceClusterService clusterService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="clusterService">The reference to service</param>
    public WorkspaceClustersController(WorkspaceClusterService clusterService)
    {
        this.clusterService = clusterService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [HttpGet]
    public Task<IEnumerable<WorkspaceClusterModel>> GetAll(string workspaceId)
    {
        return this.clusterService.GetAll(this.HttpContext.GetPrincipal().Id, workspaceId);
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    [HttpPost]
    public Task<WorkspaceClusterCreatedModel> Create(string workspaceId, [FromBody] WorkspaceClusterCreateModel input)
    {
        return this.clusterService.Create(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }
    
    /// <summary>
    /// Test a configuration for a new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The test input</param>
    /// <returns></returns>
    [HttpPost("ping")]
    public Task<WorkspaceClusterConnectionTestedModel> Ping(string workspaceId, [FromBody] WorkspaceClusterConnectionTestModel input)
    {
        return this.clusterService.Ping(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }
}

