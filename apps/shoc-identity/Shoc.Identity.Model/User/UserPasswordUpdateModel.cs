namespace Shoc.Identity.Model.User;

/// <summary>
/// The model to change the password
/// </summary>
public class UserPasswordUpdateModel
{
    /// <summary>
    /// The password to update
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The password confirmation
    /// </summary>
    public string PasswordConfirmation { get; set; }
}