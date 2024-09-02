namespace Shoc.Registry.Model.Key;

/// <summary>
/// The serializable EC key payload
/// </summary>
public class EcKeyPayload : BaseKeyPayload
{
    /// <summary>
    /// Represents the public key <see langword="Q" /> for the elliptic curve cryptography (ECC) algorithm.
    /// </summary>
    public EcKeyPointPayload Q { get; set; }
    
    /// <summary>
    /// Represents the private key <see langword="D" /> for the elliptic curve cryptography (ECC) algorithm, stored in big-endian format.
    /// </summary>
    public byte[] D { get; set; }
}