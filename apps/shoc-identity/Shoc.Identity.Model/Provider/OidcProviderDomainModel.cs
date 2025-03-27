using System;

namespace Shoc.Identity.Model.Provider;

/// <summary>
/// The OIDC provider domain model
/// </summary>
public class OidcProviderDomainModel
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
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}