namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The request object of one-time password
/// </summary>
public class OneTimePassRequest
{
    /// <summary>
    /// The target
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The delivery method
    /// </summary>
    public string DeliveryMethod { get; set; }
        
    /// <summary>
    /// The sign-in method used in context
    /// </summary>
    public string SigninMethod { get; set; }

    /// <summary>
    /// The language context
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// The return url context
    /// </summary>
    public string ReturnUrl { get; set; }
}