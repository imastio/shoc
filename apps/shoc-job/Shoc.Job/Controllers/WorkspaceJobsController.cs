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
    /// The submission service
    /// </summary>
    private readonly WorkspaceJobSubmissionService submissionService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="submissionService">The submission service</param>
    public WorkspaceJobsController(WorkspaceJobSubmissionService submissionService)
    {
        this.submissionService = submissionService;
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

