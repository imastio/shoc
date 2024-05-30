namespace Shoc.Identity.Model.Application;

/// <summary>
/// The application claim model
/// </summary>
public class ApplicationClaimModel
{
    /// <summary>
    /// The identifier
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The application id
    /// </summary>
    public string ApplicationId { get; set; }
    
    /// <summary>
    /// The type of the claim
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The claim value
    /// </summary>
    public string Value { get; set; }
    
    /// <summary>
    /// The value type of the claim
    /// </summary>
    public string ValueType { get; set; }
}