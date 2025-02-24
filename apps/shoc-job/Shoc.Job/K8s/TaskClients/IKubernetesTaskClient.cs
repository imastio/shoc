using System;
using System.Threading.Tasks;
using Shoc.Job.K8s.Model;

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
}