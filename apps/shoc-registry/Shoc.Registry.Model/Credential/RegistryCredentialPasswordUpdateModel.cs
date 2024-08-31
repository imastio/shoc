namespace Shoc.Registry.Model.Credential;

/// <summary>
/// The registry credential password update model
/// </summary>
public class RegistryCredentialPasswordUpdateModel
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
    /// The password in unencrypted form
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// The encrypted password
    /// </summary>
    public string PasswordEncrypted { get; set; }
}