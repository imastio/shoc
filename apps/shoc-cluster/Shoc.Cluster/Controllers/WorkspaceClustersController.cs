﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Cluster.Model.Cluster;
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
    /// The cluster details service
    /// </summary>
    private readonly WorkspaceClusterInstanceService clusterInstanceService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="clusterService">The reference to service</param>
    /// <param name="clusterInstanceService">The cluster details service</param>
    public WorkspaceClustersController(WorkspaceClusterService clusterService, WorkspaceClusterInstanceService clusterInstanceService)
    {
        this.clusterService = clusterService;
        this.clusterInstanceService = clusterInstanceService;
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
    /// Gets object by name
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="name">The name of object</param>
    /// <returns></returns>
    [HttpGet("by-name/{name}")]
    public Task<WorkspaceClusterModel> GetByName(string workspaceId, string name)
    {
        return this.clusterService.GetByName(this.HttpContext.GetPrincipal().Id, workspaceId, name);
    }

    /// <summary>
    /// Gets object permissions by name
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="name">The name of object</param>
    /// <returns></returns>
    [AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
    [HttpGet("by-name/{name}/permissions")]
    public Task<ISet<string>> GetPermissionsByName(string workspaceId, string name)
    {
        return this.clusterService.GetPermissionsByName(this.HttpContext.GetPrincipal().Id, workspaceId, name);
    }
    
    /// <summary>
    /// Counts all the objects
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    [HttpGet("count")]
    public Task<WorkspaceClusterCountModel> CountAll(string workspaceId)
    {
        return this.clusterService.CountAll(this.HttpContext.GetPrincipal().Id, workspaceId);
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
    public Task<ClusterConnectionTestedModel> Ping(string workspaceId, [FromBody] ClusterConnectionTestModel input)
    {
        return this.clusterInstanceService.Ping(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }
}

