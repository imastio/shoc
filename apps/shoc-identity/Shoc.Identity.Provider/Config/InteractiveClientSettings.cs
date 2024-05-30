namespace Shoc.Identity.Provider.Config;

/// <summary>
/// The interactive client settings
/// </summary>
public class InteractiveClientSettings
{
    /// <summary>
    /// The client id
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Allowed redirect URLs 
    /// </summary>
    public string RedirectPaths { get; set; }

    /// <summary>
    /// Allowed post-redirect URLs 
    /// </summary>
    public string PostLogoutRedirectPaths { get; set; }

    /// <summary>
    /// The allowed redirect URI hosts
    /// </summary>
    public string RedirectHosts { get; set; }

    /// <summary>
    /// The access token expiration in seconds
    /// </summary>
    public int? AccessTokenExpiration { get; set; }
}
