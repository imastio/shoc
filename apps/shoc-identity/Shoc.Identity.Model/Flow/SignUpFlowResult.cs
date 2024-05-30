namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign up result
/// </summary>
public class SignUpFlowResult
{
    /// <summary>
    /// The subject of authorization
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// The return url
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    /// The flow continues
    /// </summary>
    public bool ContinueFlow { get; set; }

    /// <summary>
    /// The language
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// The email of signed up user
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Indicates if email is verified
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// The confirmation is sent to the user
    /// </summary>
    public bool ConfirmationSent { get; set; }
}