using System.Threading.Tasks;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.Data;

/// <summary>
/// The repository interface
/// </summary>
public interface IJobTaskStatusRepository
{
    /// <summary>
    /// Reports the task submission by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <param name="id">The task id</param>
    /// <param name="input">The status update input</param>
    /// <returns></returns>
    Task<JobTaskModel> SubmitTaskById(string workspaceId, string jobId, string id, JobTaskSubmitInputModel input);
    
    /// <summary>
    /// Reports the task running state by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <param name="id">The task id</param>
    /// <param name="input">The input for run</param>
    /// <returns></returns>
    Task<JobTaskModel> RunningTaskById(string workspaceId, string jobId, string id, JobTaskRunningInputModel input);
    
    /// <summary>
    /// Reports the task completion by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <param name="id">The task id</param>
    /// <param name="input">The status update input</param>
    /// <returns></returns>
    Task<JobTaskModel> CompleteTaskById(string workspaceId, string jobId, string id, JobTaskCompleteInputModel input);
}