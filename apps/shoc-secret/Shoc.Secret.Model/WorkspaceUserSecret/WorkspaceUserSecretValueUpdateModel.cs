namespace Shoc.Secret.Model.WorkspaceUserSecret;

/// <summary>
/// The workspace user secret value update value model
/// </summary>
public class WorkspaceUserSecretValueUpdateModel
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
    /// Indicates if object is encrypted
    /// </summary>
    public bool Encrypted { get; set; }
    
    /// <summary>
    /// The value of the secret object
    /// </summary>
    public string Value { get; set; }
}