namespace Shoc.Identity.Model.Application;

/// <summary>
/// The application uri model
/// </summary>
public class ApplicationUriModel
{
    /// <summary>
    /// The application uri id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The application id
    /// </summary>
    public string ApplicationId { get; set; }
    
    /// <summary>
    /// The application uri type
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The uri value
    /// </summary>
    public string Uri { get; set; }
}