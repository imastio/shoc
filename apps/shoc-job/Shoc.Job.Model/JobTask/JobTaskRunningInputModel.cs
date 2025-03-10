using System;

namespace Shoc.Job.Model.JobTask;

/// <summary>
/// The job task running input
/// </summary>
public class JobTaskRunningInputModel
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
    /// The running time for the task
    /// </summary>
    public DateTime RunningAt { get; set; }
}