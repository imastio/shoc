namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The result of OTP request
/// </summary>
public class OneTimePassRequestResult
{
    /// <summary>
    /// Indicates if requested
    /// </summary>
    public bool Sent { get; set; }

    /// <summary>
    /// The delivery method
    /// </summary>
    public string DeliveryMethod { get; set; }
}