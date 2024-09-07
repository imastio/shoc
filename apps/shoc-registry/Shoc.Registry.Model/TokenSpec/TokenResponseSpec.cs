using System.Text.Json.Serialization;

namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The token response specification
/// </summary>
public class TokenResponseSpec : ITokenResponseSpec
{   
    /// <summary>
    /// The access token provided by the authorization server (other name for compatability)
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    /// <summary>
    /// The access token provided by the authorization server
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    /// <summary>
    /// The time in seconds indicating when the token will expire
    /// </summary>
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }
    
    /// <summary>
    /// The time indicating the issuing time of the token
    /// </summary>
    [JsonPropertyName("issued_at")]
    public string IssuedAt { get; set; }
    
    /// <summary>
    /// The requested scope (maybe multiple scopes)
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
    
    /// <summary>
    /// The refresh token to get a new token (optional)
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}