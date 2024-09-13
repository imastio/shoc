namespace Shoc.Core.Security;

/// <summary>
/// The password hashing algorithm
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Gets the algorithm key
    /// </summary>
    /// <returns></returns>
    string GetAlgorithm();

    /// <summary>
    /// Build a hash for given plain password
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <returns></returns>
    PasswordHash Hash(string password);

    /// <summary>
    /// Check if password matches the hash
    /// </summary>
    /// <param name="hash">The stored hash of password</param>
    /// <param name="password">The candidate password to check</param>
    /// <returns></returns>
    PasswordCheckResult Check(string hash, string password);
}
