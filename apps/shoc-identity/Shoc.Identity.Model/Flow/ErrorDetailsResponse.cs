namespace Shoc.Identity.Model.Flow;

/// <summary>
/// The error details response
/// </summary>
public class ErrorDetailsResponse
{
    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    /// <value>
    /// The error code.
    /// </value>
    public string Error { get; set; } = default!;

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    /// <value>
    /// The error description.
    /// </value>
    public string ErrorDescription { get; set; }
    
    /// <summary>
    /// The client id making the request (if available).
    /// </summary>
    public string ClientId { get; set; }
}