namespace Shoc.Job.Model.JobGitRepo;

/// <summary>
/// The job git repo model
/// </summary>
public class JobGitRepoModel
{
    /// <summary>
    /// The job label id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The job id
    /// </summary>
    public string JobId { get; set; }
    
    /// <summary>
    /// The repo id
    /// </summary>
    public string GitRepoId { get; set; }
}