using System;

namespace Shoc.Job.Model.GitRepo;

/// <summary>
/// The git repository model
/// </summary>
public class GitRepoModel
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
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}