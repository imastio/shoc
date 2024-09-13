namespace Shoc.Mailing.Model;

/// <summary>
/// The content resource for email
/// </summary>
public class ContentResource
{
    /// <summary>
    /// The content id to use
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The path to the file
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// The content type
    /// </summary>
    public string Type { get; set; }
}
