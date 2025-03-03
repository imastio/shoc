using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.Data.Sql;

/// <summary>
/// The repository interface implementation
/// </summary>
public class JobTaskStatusRepository : IJobTaskStatusRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public JobTaskStatusRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Reports the task submission by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <param name="id">The task id</param>
    /// <param name="input">The input for submission</param>
    /// <returns></returns>
    public Task<JobTaskModel> SubmitTaskById(string workspaceId, string jobId, string id, JobTaskSubmitInputModel input)
    {
        // ensure referring to the right object
        input.WorkspaceId = workspaceId;
        input.JobId = jobId;
        input.Id = id;
        
        // build the argument
        var arg = new {
            input.WorkspaceId,
            input.JobId,
            input.Id,
            input.PendingAt,
            input.Message,
            PendingTaskStatus = JobTaskStatuses.PENDING,
            PendingJobStatus = JobStatuses.PENDING,
            TargetJobStatuses = new [] {JobStatuses.CREATED}
        };
        
        return this.dataOps.Connect().QueryFirst("Job.TaskStatus", "SubmitTaskById").ExecuteAsync<JobTaskModel>(arg);
    }
    
    /// <summary>
    /// Reports the task completion by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <param name="id">The task id</param>
    /// <param name="input">The input for submission</param>
    /// <returns></returns>
    public Task<JobTaskModel> CompleteTaskById(string workspaceId, string jobId, string id, JobTaskCompleteInputModel input)
    {
        // ensure referring to the right object
        input.WorkspaceId = workspaceId;
        input.JobId = jobId;
        input.Id = id;
        
        // build the argument
        var arg = new {
            input.WorkspaceId,
            input.JobId,
            input.Id,
            input.CompletedAt,
            input.Message,
            input.Status,
            SucceededTaskIncrement = input.Status == JobTaskStatuses.SUCCEEDED ? 1 : 0,
            FailedTaskIncrement = input.Status == JobTaskStatuses.FAILED ? 1 : 0,
            CancelledTaskIncrement = input.Status == JobTaskStatuses.CANCELLED ? 1 : 0,
            SucceededJobStatus = JobStatuses.SUCCEEDED,
            PartiallySucceededJobStatus = JobStatuses.PARTIALLY_SUCCEEDED,
            FailedJobStatus = JobStatuses.FAILED,
            CancelledJobStatus = JobStatuses.CANCELLED
        };
        
        return this.dataOps.Connect().QueryFirst("Job.TaskStatus", "CompleteTaskById").ExecuteAsync<JobTaskModel>(arg);
    }
}