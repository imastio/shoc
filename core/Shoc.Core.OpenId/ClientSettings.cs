namespace Shoc.Core.OpenId;

/// <summary>
/// The client settings
/// </summary>
public class ClientSettings
{
    /// <summary>
    /// The client id
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// The client secret in the system
    /// </summary>
    public string ClientSecret { get; set; }

    /// <summary>
    /// The default scope to connect to
    /// </summary>
    public string Scope { get; set; }
}