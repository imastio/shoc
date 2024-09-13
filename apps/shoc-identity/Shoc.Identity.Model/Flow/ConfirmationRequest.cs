namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The confirmation request
/// </summary>
public class ConfirmationRequest
{
    /// <summary>
    /// The target to confirm 
    /// </summary>
    public string Target { get; set; }
        
    /// <summary>
    /// The target type
    /// </summary>
    public string TargetType { get; set; }

    /// <summary>
    /// The return url
    /// </summary>
    public string ReturnUrl { get; set; }
        
    /// <summary>
    /// The language
    /// </summary>
    public string Lang { get; set; }
}