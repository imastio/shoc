using System.Security.Cryptography;

namespace Shoc.Registry.Model.Key;

/// <summary>
/// The serializable RSA key payload
/// </summary>
public class RsaKeyPayload : BaseKeyPayload
{
    /// <summary>
    /// Represents the <see langword="D" /> parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.
    /// </summary>
    public byte[] D { get; set; }

    /// <summary>
    /// Represents the <see langword="DP" /> parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.
    /// </summary>
    public byte[] DP { get; set; }

    /// <summary>
    /// Represents the <see langword="DQ" /> parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.
    /// </summary>
    public byte[] DQ { get; set; }

    /// <summary>
    /// Represents the <see langword="Exponent" /> parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.
    /// </summary>
    public byte[] Exponent { get; set; }

    /// <summary>
    /// Represents the <see langword="InverseQ" /> parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.
    /// </summary>
    public byte[] InverseQ { get; set; }

    /// <summary>
    /// Represents the <see langword="Modulus" /> parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.
    /// </summary>
    public byte[] Modulus { get; set; }

    /// <summary>
    /// Represents the <see langword="P" /> parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.
    /// </summary>
    public byte[] P { get; set; }

    /// <summary>
    /// Represents the <see langword="Q" /> parameter for the <see cref="T:System.Security.Cryptography.RSA" /> algorithm.
    /// </summary>
    public byte[] Q { get; set; }
}