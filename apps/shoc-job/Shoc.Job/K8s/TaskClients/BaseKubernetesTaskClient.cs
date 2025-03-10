using System;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Job.K8s.Model;
using Shoc.Job.Model;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.K8s.TaskClients;

/// <summary>
/// The base Kubernetes task client
/// </summary>
public abstract class BaseKubernetesTaskClient : KubernetesClientBase, IKubernetesTaskClient
{
    /// <summary>
    /// The default restart policy
    /// </summary>
    protected const string DEFAULT_RESTART_POLICY = "Never";
    
    /// <summary>
    /// The task runtime type
    /// </summary>
    protected readonly string type;

    /// <summary>
    /// Creates new instance of base kubernetes task client
    /// </summary>
    /// <param name="config">The cluster configuration</param>
    /// <param name="type">The task runtime type</param>
    protected BaseKubernetesTaskClient(string config, string type) : base(config)
    {
        this.type = type;
    }

    /// <summary>
    /// Checks if cluster supports the given task type
    /// </summary>
    /// <returns></returns>
    public abstract Task<bool> IsSupported();

    /// <summary>
    /// Ensure the cluster can run the task of given type
    /// </summary>
    /// <returns></returns>
    public virtual async Task EnsureSupported()
    {
        // if supported just return
        if (await this.IsSupported())
        {
            return;
        }
        
        // report not supported
        throw ErrorDefinition.Validation(JobErrors.INVALID_CLUSTER, $"The task of type '{this.type}' cannot be run on the cluster").AsException();
    }
    
    /// <summary>
    /// Submits the task with given input to the cluster
    /// </summary>
    /// <param name="input">The task input</param>
    /// <returns></returns>
    public abstract Task<InitTaskResult> Submit(InitTaskInput input);

    /// <summary>
    /// Gets the task status in the cluster
    /// </summary>
    /// <param name="job">The job instance</param>
    /// <param name="task">The task instance</param>
    /// <returns></returns>
    public abstract Task<TaskK8sStatusResult> GetTaskStatus(JobModel job, JobTaskModel task);
    
    /// <summary>
    /// Disposes the client
    /// </summary>
    public void Dispose()
    {
        this.client?.Dispose();
        GC.SuppressFinalize(this);
    }
}