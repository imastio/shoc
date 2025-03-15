using System;
using System.IO;
using System.Threading.Tasks;
using Shoc.Job.K8s.Model;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.K8s.TaskClients;

/// <summary>
/// The Kubernetes task client
/// </summary>
public interface IKubernetesTaskClient : IDisposable
{
    /// <summary>
    /// Checks if cluster supports the given task type
    /// </summary>
    /// <returns></returns>
    Task<bool> IsSupported();

    /// <summary>
    /// Ensure the cluster can run the task of given type
    /// </summary>
    /// <returns></returns>
    Task EnsureSupported();

    /// <summary>
    /// Submits the task with given input to the cluster
    /// </summary>
    /// <param name="input">The task input</param>
    /// <returns></returns>
    Task<InitTaskResult> Submit(InitTaskInput input);

    /// <summary>
    /// Gets the task status in the cluster
    /// </summary>
    /// <param name="job">The job instance</param>
    /// <param name="task">The task instance</param>
    /// <returns></returns>
    Task<TaskK8sStatusResult> GetTaskStatus(JobModel job, JobTaskModel task);

    /// <summary>
    /// Gets the task logs
    /// </summary>
    /// <param name="job">The job</param>
    /// <param name="task">The task</param>
    /// <returns></returns>
    Task<Stream> GetTaskLogs(JobModel job, JobTaskModel task);
}