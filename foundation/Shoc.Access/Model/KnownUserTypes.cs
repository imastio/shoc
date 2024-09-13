namespace Shoc.Access.Model;

/// <summary>
/// The role definitions known in the system
/// </summary>
public static class KnownUserTypes
{
    /// <summary>
    /// The root user type
    /// </summary>
    public const string ROOT = "root";

    /// <summary>
    /// The type of administrator
    /// </summary>
    public const string ADMIN = "admin";

    /// <summary>
    /// The type of internal user
    /// </summary>
    public const string INTERNAL = "internal";

    /// <summary>
    /// The type of external user
    /// </summary>
    public const string EXTERNAL = "external";
}