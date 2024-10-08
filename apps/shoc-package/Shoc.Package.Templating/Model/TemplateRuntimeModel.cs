namespace Shoc.Package.Templating.Model;

/// <summary>
/// The template runtime model
/// </summary>
public class TemplateRuntimeModel
{
    /// <summary>
    /// The type of runtime
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// Indicates if supplying extra arguments is permitted
    /// </summary>
    public bool Args { get; set; }
}