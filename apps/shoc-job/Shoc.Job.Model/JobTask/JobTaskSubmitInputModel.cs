using System;

namespace Shoc.Job.Model.JobTask;

/// <summary>
/// The job task submit input
/// </summary>
public class JobTaskSubmitInputModel
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The job id
    /// </summary>
    public string JobId { get; set; }
    
    /// <summary>
    /// The task id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The message
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// The pending time for the task
    /// </summary>
    public DateTime PendingAt { get; set; }
}