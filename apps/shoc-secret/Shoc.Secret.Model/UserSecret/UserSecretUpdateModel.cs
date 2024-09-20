namespace Shoc.Secret.Model.UserSecret;

/// <summary>
/// The user secret update model
/// </summary>
public class UserSecretUpdateModel
{
    /// <summary>
    /// The id of the user secret in the system
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace associated with the user secret
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The user associated with the user secret
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The name of the user secret (should be unique within the workspace)
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The description of the user secret
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Indicates if user secret is disabled
    /// </summary>
    public bool Disabled { get; set; }
}