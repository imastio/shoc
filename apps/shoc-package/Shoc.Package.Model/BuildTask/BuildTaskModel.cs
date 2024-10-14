using System;

namespace Shoc.Package.Model.BuildTask;

/// <summary>
/// The build task model
/// </summary>
public class BuildTaskModel
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
    /// The user initiating the build
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The build task type
    /// </summary>
    public string Provider { get; set; }
    
    /// <summary>
    /// The scope of the target package
    /// </summary>
    public string Scope { get; set; }
    
    /// <summary>
    /// The listing checksum
    /// </summary>
    public string ListingChecksum { get; set; }
    
    /// <summary>
    /// The build manifest (template, spec, etc.)
    /// </summary>
    public string Manifest { get; set; }

    /// <summary>
    /// The runtime info for the package
    /// </summary>
    public string Runtime { get; set; }
    
    /// <summary>
    /// The effective Containerfile of the package
    /// </summary>
    public string Containerfile { get; set; }
    
    /// <summary>
    /// The reference to the base template
    /// </summary>
    public string TemplateReference { get; set; }
    
    /// <summary>
    /// The registry id
    /// </summary>
    public string RegistryId { get; set; }
    
    /// <summary>
    /// The status of the build task
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The deadline of the current status
    /// </summary>
    public DateTime? Deadline { get; set; }
    
    /// <summary>
    /// The target package id on success
    /// </summary>
    public string PackageId { get; set; }
    
    /// <summary>
    /// The error code if any
    /// </summary>
    public string ErrorCode { get; set; }
    
    /// <summary>
    /// The completion message
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}