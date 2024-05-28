namespace Shoc.Mailing.Model;

/// <summary>
/// The input to update profile password
/// </summary>
public class UpdateProfilePasswordInput
{
    /// <summary>
    /// The id of profile
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The plain text password to update
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The encrypted password to update
    /// </summary>
    public string PasswordEncrypted { get; set; }
}
