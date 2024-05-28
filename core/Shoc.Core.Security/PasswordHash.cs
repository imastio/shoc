namespace Shoc.Core.Security;

/// <summary>
/// The password hash result
/// </summary>
public class PasswordHash
{
    /// <summary>
    /// The algorithm type
    /// </summary>
    public string Alg { get; set; }

    /// <summary>
    /// The password hash
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The number of iterations
    /// </summary>
    public int Iterations { get; set; }

    /// <summary>
    /// The salt
    /// </summary>
    public string Salt { get; set; }

    /// <summary>
    /// Build complete hash from password
    /// </summary>
    /// <returns></returns>
    public string AsHash()
    {
        return this.ToString();
    }

    /// <summary>
    /// Gets the string representation
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{this.Alg}.{this.Iterations}.{this.Salt}.{this.Password}";
    }
}
