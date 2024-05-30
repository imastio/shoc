namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The request object for password recovery
/// </summary>
public class PasswordRecoveryRequest
{
    /// <summary>
    /// The target
    /// </summary>
    public string Email { get; set; }
        
    /// <summary>
    /// The language context
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// The return url context
    /// </summary>
    public string ReturnUrl { get; set; }
}