namespace Shoc.Registry.Model.Credential;

/// <summary>
/// The registry credential create model
/// </summary>
public class RegistryCredentialCreateModel
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
    /// The id of workspace associated with the credential
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The associated user id
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The source of the 
    /// </summary>
    public string Source { get; set; }
    
    /// <summary>
    /// The username 
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// The password in unencrypted form
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// The encrypted password
    /// </summary>
    public string PasswordEncrypted { get; set; }
    
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