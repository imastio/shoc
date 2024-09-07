using System.Text.Json.Serialization;

namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The claims for serving as a payload in the  access token 
/// </summary>
public class AccessTokenClaimsSpec
{
    /// <summary>
    /// The issuer claim indicating who issued the token
    /// </summary>
    [JsonPropertyName("iss")]
    public string Issuer { get; set; }
    
    /// <summary>
    /// The subject indicating the actual actor that token is issued to
    /// </summary>
    [JsonPropertyName("sub")]
    public string Subject { get; set; }
    
    /// <summary>
    /// The audience indicating the party who can use the token
    /// </summary>
    [JsonPropertyName("aud")]
    public string Audience { get; set; }
    
    /// <summary>
    /// The expiration time of the token
    /// </summary>
    [JsonPropertyName("exp")]
    public long Expiration { get; set; }
    
    /// <summary>
    /// The time indicating the start time of validity period for the token
    /// </summary>
    [JsonPropertyName("nbf")]
    public long NotBefore { get; set; }
    
    /// <summary>
    /// The time indicating the issuing time of the token
    /// </summary>
    [JsonPropertyName("iat")]
    public long IssuedAt { get; set; }
    
    /// <summary>
    /// The unique identifier of the token
    /// </summary>
    [JsonPropertyName("jti")]
    public string JwtId { get; set; }
}