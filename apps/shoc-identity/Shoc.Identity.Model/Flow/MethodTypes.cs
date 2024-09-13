namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The known sign-in method types
/// </summary>
public static class MethodTypes
{
    /// <summary>
    /// The password sign-in method
    /// </summary>
    public const string PASSWORD = "password";

    /// <summary>
    /// The magic link sign-in method
    /// </summary>
    public const string MAGIC_LINK = "magic_link";

    /// <summary>
    /// The one-time password sign-in type
    /// </summary>
    public const string OTP = "otp";
}