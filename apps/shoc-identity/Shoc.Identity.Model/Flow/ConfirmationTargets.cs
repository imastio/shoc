namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The known confirmation targets
/// </summary>
public static class ConfirmationTargets
{
    /// <summary>
    /// The email target for confirmation
    /// </summary>
    public const string EMAIL = "email";

    /// <summary>
    /// The phone confirmation target
    /// </summary>
    public const string PHONE = "phone";

    /// <summary>
    /// The password recovery
    /// </summary>
    public const string PASSWORD = "password";
}