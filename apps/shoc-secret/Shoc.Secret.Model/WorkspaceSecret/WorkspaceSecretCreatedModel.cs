namespace Shoc.Secret.Model.WorkspaceSecret;

/// <summary>
/// The workspace secret created model
/// </summary>
public class WorkspaceSecretCreatedModel
{
    /// <summary>
    /// The id of the secret in the system
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace associated with the secret
    /// </summary>
    public string WorkspaceId { get; set; }
}