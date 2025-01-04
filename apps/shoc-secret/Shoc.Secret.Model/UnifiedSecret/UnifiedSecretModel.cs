using System;

namespace Shoc.Secret.Model.UnifiedSecret;

/// <summary>
/// The unified secret model
/// </summary>
public class UnifiedSecretModel
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
    /// The secret kind
    /// </summary>
    public string Kind { get; set; }
    
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
    
    /// <summary>
    /// Indicates if object is encrypted
    /// </summary>
    public bool Encrypted { get; set; }
    
    /// <summary>
    /// The value of the secret object
    /// </summary>
    public string Value { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}