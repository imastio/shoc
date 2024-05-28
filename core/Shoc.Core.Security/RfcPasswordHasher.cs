using System;
using System.Linq;
using System.Security.Cryptography;

namespace Shoc.Core.Security;

/// <summary>
/// The RFC-based password hasher implementation
/// </summary>
public class RfcPasswordHasher : IPasswordHasher
{
    /// <summary>
    /// The salt size defaults to 16 (128 bit)
    /// </summary>
    private const int SALT_SIZE = 16;

    /// <summary>
    /// The key size defaults to 32 (256 bit)
    /// </summary>
    private const int KEY_SIZE = 32;

    /// <summary>
    /// The algorithm
    /// </summary>
    private readonly string alg;

    /// <summary>
    /// The hashing settings
    /// </summary>
    private readonly int iterations;

    /// <summary>
    /// Creates new RFC password hasher instance
    /// </summary>
    /// <param name="iterations">The number of iterations</param>
    public RfcPasswordHasher(int iterations)
    {
        this.alg = "RFC";
        this.iterations = iterations;
    }

    /// <summary>
    /// Gets the algorithm code
    /// </summary>
    /// <returns></returns>
    public string GetAlgorithm()
    {
        return this.alg;
    }

    /// <summary>
    /// Build a hash for given plain password
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <returns></returns>
    public PasswordHash Hash(string password)
    {
        // using the RFC-2898 algorithm
        using var algorithm = new Rfc2898DeriveBytes(password, SALT_SIZE, this.iterations, HashAlgorithmName.SHA512);

        // gets the key in base64
        var key = Convert.ToBase64String(algorithm.GetBytes(KEY_SIZE));

        // gets the salt in base64
        var salt = Convert.ToBase64String(algorithm.Salt);

        // encode hashed results with metadata
        return new PasswordHash
        {
            Alg = this.alg,
            Iterations = this.iterations,
            Salt = salt,
            Password = key
        };
    }

    /// <summary>
    /// Check if password matches the hash
    /// </summary>
    /// <param name="hash">The stored hash of password</param>
    /// <param name="password">The candidate password to check</param>
    /// <returns></returns>
    public PasswordCheckResult Check(string hash, string password)
    {
        // break password into parts
        var parts = hash.Split('.', 4);

        // password is malformed since not verified
        if (parts.Length != 4)
        {
            return new PasswordCheckResult { Verified = false };
        }

        // get algorithm
        var algCode = parts[0];

        // algorithms does not match
        if (!string.Equals(this.alg, algCode, StringComparison.CurrentCultureIgnoreCase))
        {
            return new PasswordCheckResult { Verified = false };
        }

        // get iterations part
        var passwordIterations = int.TryParse(parts[1], out var parsed) ? parsed : -1;

        // iterations are not given
        if (passwordIterations == -1)
        {
            return new PasswordCheckResult { Verified = false };
        }

        // get the salt part
        var salt = Convert.FromBase64String(parts[2]);

        // get the key part
        var key = Convert.FromBase64String(parts[3]);

        // password needs upgrade if number of iterations does not match
        var needsUpgrade = passwordIterations != this.iterations;

        // using RFC-2898 algorithm
        using var algorithm = new Rfc2898DeriveBytes(password, salt, passwordIterations, HashAlgorithmName.SHA512);

        // key to check
        var keyToCheck = algorithm.GetBytes(KEY_SIZE);

        // check if verification is successful
        var verified = keyToCheck.SequenceEqual(key);

        // build verification result
        return new PasswordCheckResult { Verified = verified, NeedsUpgrade = needsUpgrade };
    }
}
