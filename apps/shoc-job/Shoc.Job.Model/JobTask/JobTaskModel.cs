using System;

namespace Shoc.Job.Model.JobTask;

/// <summary>
/// The job task model
/// </summary>
public class JobTaskModel
{
    /// <summary>
    /// The task identifier
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The parent job identifier
    /// </summary>
    public string JobId { get; set; }
    
    /// <summary>
    /// The task sequence
    /// </summary>
    public long Sequence { get; set; }
    
    /// <summary>
    /// The target cluster to submit the job on
    /// </summary>
    public string ClusterId { get; set; }
    
    /// <summary>
    /// The package id to run
    /// </summary>
    public string PackageId { get; set; }
    
    /// <summary>
    /// The submitting user id
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The task type based on the runtime
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The runtime object json
    /// </summary>
    public string Runtime { get; set; }
    
    /// <summary>
    /// A JSON object defining CLI arguments and usage mode
    /// </summary>
    public string Args { get; set; }
    
    /// <summary>
    /// The package reference in encrypted json form
    /// </summary>
    public string PackageReferenceEncrypted { get; set; }
    
    /// <summary>
    /// Overall number of replicas
    /// </summary>
    public long ArrayReplicas { get; set; }
    
    /// <summary>
    /// The indexer for the replicas
    /// </summary>
    public string ArrayIndexer { get; set; }
    
    /// <summary>
    /// The counter for the replicas
    /// </summary>
    public string ArrayCounter { get; set; }
    
    /// <summary>
    /// The encrypted JSON with all the environment values (injected and referenced)
    /// </summary>
    public string ResolvedEnvEncrypted { get; set; }
    
    /// <summary>
    /// The amount of memory requested in bytes
    /// </summary>
    public long? MemoryRequested { get; set; }
    
    /// <summary>
    /// The amount of CPU requested
    /// </summary>
    public long? CpuRequested { get; set; }
    
    /// <summary>
    /// The amount Nvidia GPU requested
    /// </summary>
    public long? NvidiaGpuRequested { get; set; }
    
    /// <summary>
    /// The amount AMD GPU requested
    /// </summary>
    public long? AmdGpuRequested { get; set; }
    
    /// <summary>
    /// The task specification object json
    /// </summary>
    public string Spec { get; set; }
    
    /// <summary>
    /// The status of the task 
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The message of the current status
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// The pending status assigned at
    /// </summary>
    public DateTime? PendingAt { get; set; }
    
    /// <summary>
    /// The running status assigned at
    /// </summary>
    public DateTime? RunningAt { get; set; }
    
    /// <summary>
    /// The completion status (failed, cancelled, succeeded) at 
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}