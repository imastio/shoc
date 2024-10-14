namespace Shoc.Package.Model.Command;

/// <summary>
/// The container remove image context
/// </summary>
public class ContainerRmiContext
{
    /// <summary>
    /// The target image to remove
    /// </summary>
    public string Image { get; set; }
}