using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Shoc.Job.K8s.Model;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.K8s.TaskClients;

/// <summary>
/// The function Kubernetes task client
/// </summary>
public class FunctionKubernetesTaskClient : BaseKubernetesTaskClient
{
    /// <summary>
    /// Creates new instance of Function task client
    /// </summary>
    /// <param name="config">The cluster configuration</param>
    public FunctionKubernetesTaskClient(string config) : base(config, JobTaskTypes.FUNCTION)
    {
    }

    /// <summary>
    /// Checks if cluster supports the given task type
    /// </summary>
    /// <returns></returns>
    public override Task<bool> IsSupported()
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Submits the task with given input to the cluster
    /// </summary>
    /// <param name="input">The task input</param>
    /// <returns></returns>
    public override async Task<InitTaskResult> Submit(InitTaskInput input)
    {
        // the instance name
        var instanceName = $"shoc-task-{input.Task.Sequence}";
        
        // the default labels
        var labels = CreateManagedLabels(new ManagedMetadata
        {
            Name = instanceName,
            Component = ShocK8sComponents.TASK,
            PartOf = ShocK8sComponents.JOB,
            WorkspaceId = input.Task.WorkspaceId,
            JobId = input.Task.JobId,
            TaskId = input.Task.Id
        });
        
        // build the object to submit
        var instance = new V1Job
        {
            ApiVersion = "batch/v1",
            Kind = "Job",
            Metadata = new V1ObjectMeta
            {
                Name = instanceName,
                NamespaceProperty = input.Namespace,
                Labels = labels
            },
            Spec = new V1JobSpec
            {
                Template = new V1PodTemplateSpec
                {
                    Metadata = new V1ObjectMeta
                    {
                        Labels = new Dictionary<string, string>(labels)
                        {
                            [ShocK8sLabels.SHOC_POD_ROLE] = ShocK8sPodRoles.EXECUTOR
                        }
                    },
                    Spec = new V1PodSpec
                    {
                        RestartPolicy = DEFAULT_RESTART_POLICY,
                        ImagePullSecrets = GetPullSecrets(input),
                        Containers = new List<V1Container>
                        {
                            new()
                            {
                                EnvFrom = GetEnvSources(input),
                                Name = $"{instanceName}-container",
                                Image = input.PullSecret.Image,
                                Env = GetIndexerVars(input.Task),
                                Resources = new V1ResourceRequirements
                                {
                                    Requests = GetTaskResources(input.Task),
                                    Limits = GetTaskResources(input.Task)
                                },
                                SecurityContext = GetSecurityContext(input.Runtime)
                            }
                        },
                        ServiceAccountName = input.ServiceAccount,
                        SecurityContext = GetPodSecurityContext(input.Runtime)
                    }
                }
            }
        };

        // submit the object to the cluster
        await this.client.CreateNamespacedJobAsync(instance, input.Namespace);
        
        return new InitTaskResult();
    }
    
    /// <summary>
    /// Gets the task status in the cluster
    /// </summary>
    /// <param name="job">The job instance</param>
    /// <param name="task">The task instance</param>
    /// <returns></returns>
    public override async Task<TaskK8sStatusResult> GetTaskStatus(JobModel job, JobTaskModel task)
    {
        // get the target job
        var batchJobs = await this.GetKubernetesJob(job, task);

        // the target object is not found
        if (batchJobs == null || batchJobs.Count == 0)
        {
            return new TaskK8sStatusResult
            {
                ObjectState = K8sObjectState.NOT_FOUND
            };
        }
        
        // a duplicate object is detected
        if (batchJobs.Count > 1)
        {
            return new TaskK8sStatusResult
            {
                ObjectState = K8sObjectState.DUPLICATE_OBJECT
            };
        }

        // the first object
        var batchJob = batchJobs.First();
        
        return new TaskK8sStatusResult
        {
            ObjectState = K8sObjectState.OK,
            StartTime = batchJob.Status.StartTime,
            CompletionTime = batchJob.Status.CompletionTime,
            Succeeded = batchJob.Status.Succeeded is > 0
        };
    }
}