using System.Collections.Generic;

namespace Shoc.Package.Model;

/// <summary>
/// The build manifest model
/// </summary>
public class BuildManifestModel
{
    /// <summary>
    /// The template (name and variant)
    /// </summary>
    public string Template { get; set; }
    
    /// <summary>
    /// The build specification
    /// </summary>
    public Dictionary<string, object> Spec { get; set; }
    
    /// <summary>
    /// The patterns to ignore while building
    /// </summary>
    public string[] Ignore { get; set; }
}