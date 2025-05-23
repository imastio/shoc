namespace Shoc.Identity.Model.Provider;

/// <summary>
/// The OIDC provider domain update model
/// </summary>
public class OidcProviderDomainUpdateModel
{
    /// <summary>
    /// The domain id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The provider id
    /// </summary>
    public string ProviderId { get; set; }
    
    /// <summary>
    /// The domain name
    /// </summary>
    public string DomainName { get; set; }
    
    /// <summary>
    /// Indicates if domain is verified
    /// </summary>
    public bool Verified { get; set; }
}