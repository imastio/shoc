namespace Shoc.Secret.Model.WorkspaceSecret;

/// <summary>
/// The workspace secret deleted model
/// </summary>
public class WorkspaceSecretDeletedModel
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