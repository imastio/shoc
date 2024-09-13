namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The authenticated subject 
/// </summary>
public class AuthenticatedSubject
{
    /// <summary>
    /// The registry id
    /// </summary>
    public string RegistryId { get; set; }
    
    /// <summary>
    /// The credential id
    /// </summary>
    public string CredentialId { get; set; }
    
    /// <summary>
    /// The authenticated username
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// The workspace of authenticated party
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The assigned user id
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// Indicates if pull is allowed
    /// </summary>
    public bool PullAllowed { get; set; }
    
    /// <summary>
    /// Indicates if push allowed
    /// </summary>
    public bool PushAllowed { get; set; }
}