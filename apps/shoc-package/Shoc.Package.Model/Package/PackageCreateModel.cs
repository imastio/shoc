using System;

namespace Shoc.Package.Model.Package;

/// <summary>
/// The package model
/// </summary>
public class PackageCreateModel
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
    /// The owner of the package
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The target scope
    /// </summary>
    public string Scope { get; set; }
    
    /// <summary>
    /// The manifest of the package
    /// </summary>
    public string Manifest { get; set; }
    
    /// <summary>
    /// The runtime of the package
    /// </summary>
    public string Runtime { get; set; }
    
    /// <summary>
    /// The package checksum
    /// </summary>
    public string ListingChecksum { get; set; }
    
    /// <summary>
    /// The effective dockerfile of the package
    /// </summary>
    public string Dockerfile { get; set; }
    
    /// <summary>
    /// The reference to the base template
    /// </summary>
    public string TemplateReference { get; set; }
    
    /// <summary>
    /// The registry id
    /// </summary>
    public string RegistryId { get; set; }
    
    /// <summary>
    /// The full url to the image
    /// </summary>
    public string Image { get; set; }
}