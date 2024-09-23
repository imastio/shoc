namespace Shoc.Secret.Model.WorkspaceUserSecret;

/// <summary>
/// The workspace user secret created model
/// </summary>
public class WorkspaceUserSecretCreatedModel
{
    /// <summary>
    /// The id of the secret in the system
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace associated with the secret
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The associated user id
    /// </summary>
    public string UserId { get; set; }
}