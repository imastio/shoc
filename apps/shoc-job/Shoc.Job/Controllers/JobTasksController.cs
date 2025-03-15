using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Job.Model;
using Shoc.Job.Model.JobTask;
using Shoc.Job.Services;

namespace Shoc.Job.Controllers;

/// <summary>
/// The jobs endpoint
/// </summary>
[Route("api/management/workspaces/{workspaceId}/jobs/{jobId}/tasks")]
[ApiController]
[ShocExceptionHandler]
public class JobTasksController : ControllerBase
{
    /// <summary>
    /// The job task service
    /// </summary>
    private readonly JobTaskService jobTaskService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="jobTaskService">The reference to service</param>
    public JobTasksController(JobTaskService jobTaskService)
    {
        this.jobTaskService = jobTaskService;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(JobAccesses.JOB_JOBS_READ)]
    [HttpGet("extended")]
    public Task<IEnumerable<JobTaskExtendedModel>> GetAllExtended(string workspaceId, string jobId)
    {
        return this.jobTaskService.GetAllExtended(workspaceId, jobId);
    }
    
    /// <summary>
    /// Gets the extended object by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}/extended")]
    [AuthorizeAnyAccess(JobAccesses.JOB_JOBS_READ)]
    public Task<JobTaskExtendedModel> GetExtendedById(string workspaceId, string jobId, string id)
    {
        return this.jobTaskService.GetExtendedById(workspaceId, jobId, id);
    }
    
    /// <summary>
    /// Gets the logs of the task
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}/logs")]
    [AuthorizeAnyAccess(JobAccesses.JOB_JOBS_READ)]
    public async Task<Stream> GetLogsById(string workspaceId, string jobId, string id)
    {
        // get the stream
        var stream = this.jobTaskService.GetLogsById(workspaceId, jobId, id);
        
        // ensure stream is disposed afterwards
        this.Response.RegisterForDispose(stream);
        
        // return the stream
        return await stream;
    }
}

