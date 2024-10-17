
namespace Shoc.Package.Templating.Model;

/// <summary>
/// The template variant model
/// </summary>
public class TemplateVariantDefinition
{
    /// <summary>
    /// The template name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The template variant
    /// </summary>
    public string Variant { get; set; }
    
    /// <summary>
    /// The template title
    /// </summary>
    public string Title { get; set; } 
    
    /// <summary>
    /// The template description
    /// </summary>
    public string Description { get; set; } 
    
    /// <summary>
    /// The template author
    /// </summary>
    public string Author { get; set; }
    
    /// <summary>
    /// The template build spec
    /// </summary>
    public string BuildSpec { get; set; } 
    
    /// <summary>
    /// The template runtime
    /// </summary>
    public TemplateRuntimeModel Runtime { get; set; } 
    
    /// <summary>
    /// The template container file
    /// </summary>
    public string Containerfile { get; set; } 
    
    /// <summary>
    /// The template overview in Markdown
    /// </summary>
    public string OverviewMarkdown { get; set; }
    
    /// <summary>
    /// The template specification markdown 
    /// </summary>
    public string SpecificationMarkdown { get; set; }
    
    /// <summary>
    /// The template examplse markdown
    /// </summary>
    public string ExamplesMarkdown { get; set; } 
}