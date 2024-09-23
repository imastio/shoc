namespace Shoc.Secret.Model.WorkspaceSecret;

/// <summary>
/// The workspace secret value update value model
/// </summary>
public class WorkspaceSecretValueUpdateModel
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
    /// Indicates if object is encrypted
    /// </summary>
    public bool Encrypted { get; set; }
    
    /// <summary>
    /// The value of the secret object
    /// </summary>
    public string Value { get; set; }
}