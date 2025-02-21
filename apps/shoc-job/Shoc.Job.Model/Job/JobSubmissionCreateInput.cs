namespace Shoc.Job.Model.Job;

/// <summary>
/// The job submission creation input
/// </summary>
public class JobSubmissionCreateInput
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The user id
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The job scope
    /// </summary>
    public string Scope { get; set; }
    
    /// <summary>
    /// The manifest object
    /// </summary>
    public JobRunManifestModel Manifest { get; set; }
}