using System;

namespace Shoc.Job.Model.Job;

/// <summary>
/// The job definition model
/// </summary>
public class JobModel
{
    /// <summary>
    /// The identifier of the job
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The key of the job within the workspace
    /// </summary>
    public long LocalId { get; set; }
    
    /// <summary>
    /// The target cluster for the job
    /// </summary>
    public string ClusterId { get; set; }
    
    /// <summary>
    /// The id of the user submitting the job
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The scope of the job (user or workspace)
    /// </summary>
    public string Scope { get; set; }
    
    /// <summary>
    /// The original run manifest
    /// </summary>
    public string Manifest { get; set; }
    
    /// <summary>
    /// The cluster configuration encrypted
    /// </summary>
    public string ClusterConfigEncrypted { get; set; }
    
    /// <summary>
    /// The namespace value for the job
    /// </summary>
    public string Namespace { get; set; }
    
    /// <summary>
    /// The total count of tasks
    /// </summary>
    public long TotalTasks { get; set; }
    
    /// <summary>
    /// The number of completed tasks
    /// </summary>
    public long CompletedTasks { get; set; }
    
    /// <summary>
    /// The status of the job 
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