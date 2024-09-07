using System.Text.Json.Serialization;

namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The token request specification
/// </summary>
public class TokenRequestSpec
{
    /// <summary>
    /// The grant type
    /// </summary>
    [JsonPropertyName("grant_type")]
    [JsonRequired]
    public string GrantType { get; set; }
    
    /// <summary>
    /// The service name
    /// </summary>
    [JsonPropertyName("service")]
    [JsonRequired]
    public string Service { get; set; }
    
    /// <summary>
    /// The client id
    /// </summary>
    [JsonPropertyName("client_id")]
    [JsonRequired]
    public string ClientId { get; set; }
    
    /// <summary>
    /// The access type (optional).
    /// The value should be "offline" if a new refresh token is requested.
    /// The value should be "online" if a refresh token is not requested (default). 
    /// </summary>
    [JsonPropertyName("access_type")]
    public string AccessType { get; set; }
    
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
    
    /// <summary>
    /// The username credential to use with the 'password' grant type
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; }
    
    /// <summary>
    /// The password credential to use with the 'password' grant type
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; }
}