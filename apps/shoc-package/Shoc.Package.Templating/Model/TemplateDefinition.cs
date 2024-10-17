using System.Collections.Generic;

namespace Shoc.Package.Templating.Model;

/// <summary>
/// The template model
/// </summary>
public class TemplateDefinition
{
    /// <summary>
    /// The template name
    /// </summary>
    public string Name { get; set; }
    
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
    /// The template variants
    /// </summary>
    public IDictionary<string, TemplateVariantDefinition> Variants { get; set; }
}