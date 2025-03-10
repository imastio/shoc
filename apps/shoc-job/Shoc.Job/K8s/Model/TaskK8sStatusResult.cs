using System;

namespace Shoc.Job.K8s.Model;

/// <summary>
/// The task status in Kubernetes
/// </summary>
public class TaskK8sStatusResult
{
    /// <summary>
    /// The object state in the cluster
    /// </summary>
    public string ObjectState { get; set; }
    
    /// <summary>
    /// The start time of the workload
    /// </summary>
    public DateTime? StartTime { get; set; }
    
    /// <summary>
    /// The completion time of the workload
    /// </summary>
    public DateTime? CompletionTime { get; set; }
    
    /// <summary>
    /// Indicates if succeeded
    /// </summary>
    public bool Succeeded { get; set; }
}