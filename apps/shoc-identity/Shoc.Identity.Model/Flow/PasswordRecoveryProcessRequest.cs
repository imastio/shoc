namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The password recovery process request
/// </summary>
public class PasswordRecoveryProcessRequest
{
    /// <summary>
    /// The email to process
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The password recovery confirmation code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// The new password for approval
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The request language
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// The return url
    /// </summary>
    public string ReturnUrl { get; set; }
}