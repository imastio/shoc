namespace Shoc.Registry.Model.Credential;

/// <summary>
/// The registry credential filter
/// </summary>
public class RegistryCredentialFilter
{
    /// <summary>
    /// Filter by workspace or not
    /// </summary>
    public bool ByWorkspace { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// Filter by user or not
    /// </summary>
    public bool ByUser { get; set; }
    
    /// <summary>
    /// The user id
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// Filter by pull access
    /// </summary>
    public bool? PullAllowed { get; set; }
    
    /// <summary>
    /// Filter by push access
    /// </summary>
    public bool? PushAllowed { get; set; }
}