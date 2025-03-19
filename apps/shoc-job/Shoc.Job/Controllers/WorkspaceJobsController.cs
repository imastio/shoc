using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.Access.Model;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.ApiCore.Auth;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.WorkspaceJob;
using Shoc.Job.Services;

namespace Shoc.Job.Controllers;

/// <summary>
/// The labels endpoint
/// </summary>
[Route("api/workspaces/{workspaceId}/jobs")]
[ApiController]
[ShocExceptionHandler]
[AuthorizeMinUserType(KnownUserTypes.EXTERNAL)]
public class WorkspaceJobsController : ControllerBase
{
    /// <summary>
    /// The job service
    /// </summary>
    private readonly WorkspaceJobService jobService;
    
    /// <summary>
    /// The submission service
    /// </summary>
    private readonly WorkspaceJobSubmissionService submissionService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="jobService">The job service</param>
    /// <param name="submissionService">The submission service</param>
    public WorkspaceJobsController(WorkspaceJobService jobService, WorkspaceJobSubmissionService submissionService)
    {
        this.jobService = jobService;
        this.submissionService = submissionService;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<JobPageResult<WorkspaceJobModel>> GetPageBy(string workspaceId, 
        [FromQuery] string userId,
        [FromQuery] string scope,
        [FromQuery] string status,
        [FromQuery] string clusterId,
        [FromQuery] bool all,
        [FromQuery] int page, 
        [FromQuery] int? size)
    {
        return this.jobService.GetPageBy(this.HttpContext.GetPrincipal().Id, workspaceId, new JobFilter
        {
            AccessibleOnly = !all,
            AccessingUserId = this.HttpContext.GetPrincipal().Id,
            UserId = userId,
            Scope = scope,
            ClusterId = clusterId,
            Status = status
        }, page, size);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public Task<WorkspaceJobModel> GetById(string workspaceId, string id)
    {
        return this.jobService.GetById(this.HttpContext.GetPrincipal().Id, workspaceId, id);
    }
    
    /// <summary>
    /// Gets the object by local id
    /// </summary>
    /// <returns></returns>
    [HttpGet("by-local-id/{id:long}")]
    public Task<WorkspaceJobModel> GetById(string workspaceId, long id)
    {
        return this.jobService.GetByLocalId(this.HttpContext.GetPrincipal().Id, workspaceId, id);
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    [HttpPost]
    public Task<WorkspaceJobCreatedModel> Create(string workspaceId, [FromBody] JobSubmissionCreateInput input)
    {
        return this.submissionService.Create(this.HttpContext.GetPrincipal().Id, workspaceId, input);
    }

    /// <summary>
    /// Submits the job object
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The job id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    [HttpPost("{id}/submit")]
    public Task<WorkspaceJobCreatedModel> Submit(string workspaceId, string id, [FromBody] JobSubmissionInput input)
    {
        return this.submissionService.Submit(this.HttpContext.GetPrincipal().Id, workspaceId, id, input);
    }
}

