namespace Shoc.Secret.Model.WorkspaceSecret;

/// <summary>
/// The workspace secret update model
/// </summary>
public class WorkspaceSecretUpdateModel
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
    /// The name of the secret (should be unique within the workspace)
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The description of the secret
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Indicates if secret is disabled
    /// </summary>
    public bool Disabled { get; set; }
}