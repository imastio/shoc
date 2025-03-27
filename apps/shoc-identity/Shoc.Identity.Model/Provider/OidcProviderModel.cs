using System;

namespace Shoc.Identity.Model.Provider;

/// <summary>
/// The OIDC provider model
/// </summary>
public class OidcProviderModel
{
    /// <summary>
    /// The provider id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The code of the provider
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// The type of the provider
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The name of the provider
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The URI to the icon
    /// </summary>
    public string IconUrl { get; set; }
    
    /// <summary>
    /// The authority URI
    /// </summary>
    public string Authority { get; set; }
    
    /// <summary>
    /// The type of the response
    /// </summary>
    public string ResponseType { get; set; }
    
    /// <summary>
    /// The client id
    /// </summary>
    public string ClientId { get; set; }
    
    /// <summary>
    /// The encrypted client secret
    /// </summary>
    public string ClientSecretEncrypted { get; set; }
    
    /// <summary>
    /// The list of scope values (delimited with spaces)
    /// </summary>
    public string Scope { get; set; }
    
    /// <summary>
    /// Indicates if we should fetch user info from the endpoint
    /// </summary>
    public bool FetchUserInfo { get; set; }
    
    /// <summary>
    /// Indicates if PKCE should be used or not
    /// </summary>
    public bool Pkce { get; set; }
    
    /// <summary>
    /// Indicates if provider is disabled
    /// </summary>
    public bool Disabled { get; set; }
    
    /// <summary>
    /// Indicates if the provider is trusted
    /// </summary>
    public bool Trusted { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}