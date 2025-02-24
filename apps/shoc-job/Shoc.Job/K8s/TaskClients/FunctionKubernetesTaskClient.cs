using System.Collections.Generic;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Shoc.Job.K8s.Model;
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
        
        // build the object to submit
        var instance = new V1Job
        {
            ApiVersion = "batch/v1",
            Kind = "Job",
            Metadata = new V1ObjectMeta
            {
                Name = instanceName,
                NamespaceProperty = input.Namespace
            },
            Spec = new V1JobSpec
            {
                Template = new V1PodTemplateSpec
                {
                    Spec = new V1PodSpec
                    {
                        RestartPolicy = DEFAULT_RESTART_POLICY,
                        ImagePullSecrets = this.GetPullSecrets(input),
                        Containers = new List<V1Container>
                        {
                            new()
                            {
                                EnvFrom = this.GetEnvSources(input),
                                Name = $"{instanceName}-container",
                                Image = input.PullSecret.Image
                            }
                        }
                    }
                }
            }
        };

        // submit the object to the cluster
        var result = await this.client.CreateNamespacedJobAsync(instance, input.Namespace);
        
        return new InitTaskResult();
    }
}