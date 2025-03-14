using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Job.Model;
using Shoc.Job.Model.Job;
using Shoc.Job.Services;

namespace Shoc.Job.Controllers;

/// <summary>
/// The jobs endpoint
/// </summary>
[Route("api/management/workspaces/{workspaceId}/jobs")]
[ApiController]
[ShocExceptionHandler]
public class JobsController : ControllerBase
{
    /// <summary>
    /// The job service
    /// </summary>
    private readonly JobService jobService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="jobService">The reference to service</param>
    public JobsController(JobService jobService)
    {
        this.jobService = jobService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_JOBS_LIST)]
    [HttpGet("extended")]
    public Task<JobPageResult<JobExtendedModel>> GetExtendedPageBy(string workspaceId, 
        [FromQuery] string userId,
        [FromQuery] string scope,
        [FromQuery] string status,
        [FromQuery] int page, 
        [FromQuery] int? size)
    {
        return this.jobService.GetExtendedPageBy(workspaceId, new JobFilter
        {
            UserId = userId,
            Scope = scope,
            Status = status
        }, page, size);
    }
    
    /// <summary>
    /// Gets the extended object by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}/extended")]
    [AuthorizeAnyAccess(JobAccesses.JOB_JOBS_READ)]
    public Task<JobExtendedModel> GetExtendedById(string workspaceId, string id)
    {
        return this.jobService.GetExtendedById(workspaceId, id);
    }
}

