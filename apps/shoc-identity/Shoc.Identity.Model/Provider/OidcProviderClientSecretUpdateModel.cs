namespace Shoc.Identity.Model.Provider;

/// <summary>
/// The OIDC provider update secret model
/// </summary>
public class OidcProviderClientSecretUpdateModel
{
    /// <summary>
    /// The provider id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The plain version of client secret
    /// </summary>
    public string ClientSecret { get; set; }
    
    /// <summary>
    /// The encrypted client secret
    /// </summary>
    public string ClientSecretEncrypted { get; set; }
}