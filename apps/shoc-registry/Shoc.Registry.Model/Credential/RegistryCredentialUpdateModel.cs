namespace Shoc.Registry.Model.Credential;

/// <summary>
/// The registry credential update model
/// </summary>
public class RegistryCredentialUpdateModel
{
    /// <summary>
    /// The id of the credential
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The id of registry
    /// </summary>
    public string RegistryId { get; set; }
    
    /// <summary>
    /// The username 
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// The email address
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// The pull operation is allowed
    /// </summary>
    public bool PullAllowed { get; set; }
    
    /// <summary>
    /// The push operation is allowed
    /// </summary>
    public bool PushAllowed { get; set; }
}