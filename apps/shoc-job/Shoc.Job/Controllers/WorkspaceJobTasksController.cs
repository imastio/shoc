using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Job.Model.WorkspaceJobTask;
using Shoc.Job.Services;

namespace Shoc.Job.Controllers;

/// <summary>
/// The labels endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/jobs/{jobId}/tasks")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class WorkspaceJobTasksController : ControllerBase
{
    /// <summary>
    /// The job tasks service
    /// </summary>
    private readonly WorkspaceJobTaskService jobTaskService;
    
    /// <summary>
    /// The log streaming service
    /// </summary>
    private readonly LogStreamingService logStreamingService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="jobTaskService">The job tasks service</param>
    /// <param name="logStreamingService">The log streaming service</param>
    public WorkspaceJobTasksController(WorkspaceJobTaskService jobTaskService, LogStreamingService logStreamingService)
    {
        this.jobTaskService = jobTaskService;
        this.logStreamingService = logStreamingService;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<IEnumerable<WorkspaceJobTaskModel>> GetAll(string workspaceId, string jobId)
    {
        return this.jobTaskService.GetAll(this.HttpContext.GetPrincipal().Id, workspaceId, jobId);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public Task<WorkspaceJobTaskModel> GetById(string workspaceId, string jobId, string id)
    {
        return this.jobTaskService.GetById(this.HttpContext.GetPrincipal().Id, workspaceId, jobId, id);
    }
    
    /// <summary>
    /// Gets the object by sequence
    /// </summary>
    /// <returns></returns>
    [HttpGet("by-sequence/{sequence:long}")]
    public Task<WorkspaceJobTaskModel> GetBySequence(string workspaceId, string jobId, long sequence)
    {
        return this.jobTaskService.GetBySequence(this.HttpContext.GetPrincipal().Id, workspaceId, jobId, sequence);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("by-sequence/{sequence:long}/logs")]
    public async Task GetLogsBySequence(string workspaceId, string jobId, long sequence, CancellationToken cancellationToken)
    {
        // get the log stream
        var stream = await this.jobTaskService.GetLogsBySequence(this.HttpContext.GetPrincipal().Id, workspaceId, jobId, sequence);
        
        // redirect the input stream to response
        await this.logStreamingService.StreamLogs(stream, this.Response, cancellationToken);
    }
}

