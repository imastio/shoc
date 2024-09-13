namespace Shoc.Mailing.Model;

/// <summary>
/// The mailing error definitions
/// </summary>
public static class MailingErrors
{
    /// <summary>
    /// The unknown error
    /// </summary>
    public const string UNKNOWN_ERROR = "MAILING_UNKNOWN_ERROR";

    /// <summary>
    /// The profile is missing
    /// </summary>
    public const string MISSING_PROFILE = "MAILING_MISSING_PROFILE";

    /// <summary>
    /// The password is missing
    /// </summary>
    public const string MISSING_PASSWORD = "MAILIGN_MISSING_PASSWORD";

    /// <summary>
    /// The credentials are invalid
    /// </summary>
    public const string INVALID_CREDENTIALS = "MAILING_INVALID_CREDENTIALS";

    /// <summary>
    /// The code is missing or invalid
    /// </summary>
    public const string INVALID_CODE = "MAILING_INVALID_CODE";

    /// <summary>
    /// The profile with the code already exists
    /// </summary>
    public const string EXISTING_CODE = "MAILING_EXISTING_CODE";

    /// <summary>
    /// The provider is invalid
    /// </summary>
    public const string INVALID_PROVIDER = "MAILING_INVALID_PROVIDER";
}
