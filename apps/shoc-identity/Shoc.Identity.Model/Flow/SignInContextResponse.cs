namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign-in context response
/// </summary>
public class SignInContextResponse
{
    /// <summary>
    /// The language identifier
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// The login hint value if set
    /// </summary>
    public string LoginHint { get; set; }
}