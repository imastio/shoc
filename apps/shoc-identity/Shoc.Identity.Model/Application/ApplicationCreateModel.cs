namespace Shoc.Identity.Model.Application;

/// <summary>
/// The application create model
/// </summary>
public class ApplicationCreateModel
{
    /// <summary>
    /// The client identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Specifies if client is enabled
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// The application client id for the client
    /// </summary>
    public string ApplicationClientId { get; set; }
    
    /// <summary>
    /// Client display name (used for logging and consent screen)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of the client.
    /// </summary>
    public string Description { get; set; }
}