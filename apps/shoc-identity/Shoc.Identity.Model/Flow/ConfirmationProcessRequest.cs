namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The confirmation process request
/// </summary>
public class ConfirmationProcessRequest
{
    /// <summary>
    /// The email of user
    /// </summary>
    public string Target { get; set; }

    /// <summary>
    /// The target type
    /// </summary>
    public string TargetType { get; set; }

    /// <summary>
    /// The confirmation code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// The request language
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    /// The return url
    /// </summary>
    public string ReturnUrl { get; set; }
}