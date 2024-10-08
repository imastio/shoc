using System;

namespace Shoc.Package.Model.Package;

/// <summary>
/// The package model
/// </summary>
public class PackageModel
{
    /// <summary>
    /// The object id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The package checksum
    /// </summary>
    public string Checksum { get; set; }
    
    /// <summary>
    /// The reference to the base template
    /// </summary>
    public string TemplateReference { get; set; }
    
    /// <summary>
    /// The effective dockerfile of the package
    /// </summary>
    public string Dockerfile { get; set; }
    
    /// <summary>
    /// The registry id
    /// </summary>
    public string RegistryId { get; set; }
    
    /// <summary>
    /// The full url to the image
    /// </summary>
    public string Image { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}