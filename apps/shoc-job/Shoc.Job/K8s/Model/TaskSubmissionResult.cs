using System;

namespace Shoc.Job.K8s.Model;

/// <summary>
/// The result of job task submission
/// </summary>
public class TaskSubmissionResult
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
    /// The operation completed successfully
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// The exception in case of failure
    /// </summary>
    public Exception Exception { get; set; }
    
    /// <summary>
    /// The task initialization result
    /// </summary>
    public InitTaskResult InitResult { get; set; }
}