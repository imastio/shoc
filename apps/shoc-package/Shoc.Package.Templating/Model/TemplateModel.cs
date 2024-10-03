namespace Shoc.Package.Templating.Model;

/// <summary>
/// The template model
/// </summary>
public class TemplateModel
{
    /// <summary>
    /// The name of the template
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The set of variants available for the template
    /// </summary>
    public string[] Variants { get; set; }
}