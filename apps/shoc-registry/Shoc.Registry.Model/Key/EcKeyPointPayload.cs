namespace Shoc.Registry.Model.Key;

/// <summary>
/// The serializable EC key point payload
/// </summary>
public class EcKeyPointPayload 
{
    /// <summary>
    /// Represents the X coordinate.
    /// </summary>
    public byte[] X { get; set; }
    
    /// <summary>
    /// Represents the Y coordinate.
    /// </summary>
    public byte[] Y { get; set; }
}