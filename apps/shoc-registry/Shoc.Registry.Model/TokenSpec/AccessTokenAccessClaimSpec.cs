using System.Text.Json.Serialization;

namespace Shoc.Registry.Model.TokenSpec;

/// <summary>
/// The claim type indicating the "access" field in the access token
/// </summary>
public class AccessTokenAccessClaimSpec
{
    /// <summary>
    /// The type of the resource (typically 'repository')
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    /// <summary>
    /// The name of the resource (example: my-repo/image-name)
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// The set of actions granted for the resource (example: [pull, push])
    /// </summary>
    [JsonPropertyName("actions")]
    public string[] Actions { get; set; }
}