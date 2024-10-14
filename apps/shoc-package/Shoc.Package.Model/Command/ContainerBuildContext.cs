namespace Shoc.Package.Model.Command;

/// <summary>
/// The container build context
/// </summary>
public class ContainerBuildContext
{
    /// <summary>
    /// The path to the containerfile
    /// </summary>
    public string Containerfile { get; set; }
    
    /// <summary>
    /// The context directory to build
    /// </summary>
    public string WorkingDirectory { get; set; }
    
    /// <summary>
    /// The target image to label
    /// </summary>
    public string Image { get; set; }
}