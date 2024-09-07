using System.Text.Json.Serialization;

namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The token error specification
/// </summary>
public class TokenErrorSpec : ITokenResponseSpec
{
    /// <summary>
    /// The error code
    /// </summary>
    [JsonPropertyName("error")]
    public string Error { get; set; }
    
    /// <summary>
    /// The error description
    /// </summary>
    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; }

    /// <summary>
    /// Creates a new error object based on the type
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns></returns>
    public static TokenErrorSpec Create(string type)
    {
        return new TokenErrorSpec
        {
            Error = type,
            ErrorDescription = TokenErrorDescriptions.DESCRIPTIONS.TryGetValue(type, out var desc) ? desc : type
        };
    }
}