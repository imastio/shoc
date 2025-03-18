namespace Shoc.Job.Model.Job;

/// <summary>
/// The job filter
/// </summary>
public class JobFilter
{
    /// <summary>
    /// Show only accessible jobs
    /// </summary>
    public bool AccessibleOnly { get; set; }
    
    /// <summary>
    /// The accessing user id
    /// </summary>
    public string AccessingUserId { get; set; }
    
    /// <summary>
    /// The owner of the job
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The target scope
    /// </summary>
    public string Scope { get; set; }
    
    /// <summary>
    /// The status of the job
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The target cluster
    /// </summary>
    public string ClusterId { get; set; }
}