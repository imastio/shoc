using System;

namespace Shoc.Job.Model.Job;

/// <summary>
/// The job failure input
/// </summary>
public class JobFailInputModel
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The job id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The message
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// The pending time for the task
    /// </summary>
    public DateTime CompletedAt { get; set; }
}