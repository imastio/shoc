namespace Shoc.Package.Model.Command;

/// <summary>
/// The container push context
/// </summary>
public class ContainerPushContext
{
    /// <summary>
    /// The target image to label
    /// </summary>
    public string Image { get; set; }
    
    /// <summary>
    /// The username
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// The password
    /// </summary>
    public string Password { get; set; }
}