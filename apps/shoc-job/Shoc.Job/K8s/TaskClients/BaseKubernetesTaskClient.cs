using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Shoc.Core;
using Shoc.Core.Kubernetes;
using Shoc.Job.K8s.Model;
using Shoc.Job.Model;

namespace Shoc.Job.K8s.TaskClients;

/// <summary>
/// The base Kubernetes task client
/// </summary>
public abstract class BaseKubernetesTaskClient : IKubernetesTaskClient
{
    /// <summary>
    /// The default restart policy
    /// </summary>
    protected const string DEFAULT_RESTART_POLICY = "Never";
    
    /// <summary>
    /// The Kubernetes client instance
    /// </summary>
    protected readonly Kubernetes client;
    
    /// <summary>
    /// The task runtime type
    /// </summary>
    protected readonly string type;

    /// <summary>
    /// Creates new instance of base kubernetes task client
    /// </summary>
    /// <param name="config">The cluster configuration</param>
    /// <param name="type">The task runtime type</param>
    protected BaseKubernetesTaskClient(string config, string type)
    {
        this.client = new Kubernetes(new KubeContext { Config = config }.AsClientConfiguration());
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
    /// Gets the list of pull secrets based on the task input
    /// </summary>
    /// <param name="input">The input</param>
    /// <returns></returns>
    protected IList<V1LocalObjectReference> GetPullSecrets(InitTaskInput input)
    {
        return new List<V1LocalObjectReference>
        {
            new()
            {
                Name = input.PullSecret.PullSecret.Name()
            }
        };
    }

    /// <summary>
    /// Gets the env sources from the input
    /// </summary>
    /// <param name="input">The task input</param>
    /// <returns></returns>
    protected IList<V1EnvFromSource> GetEnvSources(InitTaskInput input)
    {
        // the result collection
        var result = new List<V1EnvFromSource>();

        // add secrets
        result.AddRange(input.SharedEnv.Secrets.Select(sec => new V1EnvFromSource
        {
            SecretRef = new V1SecretEnvSource
            {
                Name = sec.Name()
            }
        }));
        
        // add config maps
        result.AddRange(input.SharedEnv.ConfigMaps.Select(sec => new V1EnvFromSource
        {
            ConfigMapRef = new V1ConfigMapEnvSource
            {
                Name = sec.Name()
            }
        }));
        
        // return result
        return result;
    }
    
    /// <summary>
    /// Disposes the client
    /// </summary>
    public void Dispose()
    {
        this.client?.Dispose();
        GC.SuppressFinalize(this);
    }
}