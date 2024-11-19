namespace Shoc.Job.Model.GitRepo;

/// <summary>
/// The git repository create model
/// </summary>
public class GitRepoCreateModel
{
    /// <summary>
    /// The id of the repository
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The name of the repository
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The owner of the repository
    /// </summary>
    public string Owner { get; set; }
    
    /// <summary>
    /// The source of the repository
    /// </summary>
    public string Source { get; set; }
    
    /// <summary>
    /// The repository reference
    /// </summary>
    public string Repository { get; set; }
    
    /// <summary>
    /// The url of the repository
    /// </summary>
    public string RemoteUrl { get; set; }
}