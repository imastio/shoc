namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The sign-up model
/// </summary>
public class SignUpFlowInput
{
    /// <summary>
    /// The preferred email for sign-up
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The preferred password for sign-up
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The full name for sign-up
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// The user's timezone
    /// </summary>
    public string Timezone { get; set; }

    /// <summary>
    /// The user's country
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// The context language
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// The return url
    /// </summary>
    public string ReturnUrl { get; set; }
}