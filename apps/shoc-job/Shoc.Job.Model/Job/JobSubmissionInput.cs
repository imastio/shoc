namespace Shoc.Job.Model.Job;

/// <summary>
/// The job submission input
/// </summary>
public class JobSubmissionInput
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
    /// The user id
    /// </summary>
    public string UserId { get; set; }
}