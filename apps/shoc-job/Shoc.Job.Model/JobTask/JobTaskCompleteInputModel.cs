using System;

namespace Shoc.Job.Model.JobTask;

/// <summary>
/// The job task complete input
/// </summary>
public class JobTaskCompleteInputModel
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
    /// The completing status of the task
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The message
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// The pending time for the task
    /// </summary>
    public DateTime CompletedAt { get; set; }
}