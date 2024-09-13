using System;

namespace Shoc.Registry.Model.Key;

/// <summary>
/// The serializable base key payload
/// </summary>
public abstract class BaseKeyPayload
{
    /// <summary>
    /// The key id
    /// </summary>
    public string KeyId { get; set; }
    
    /// <summary>
    /// The algorithm of the key
    /// </summary>
    public string Algorithm { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
}