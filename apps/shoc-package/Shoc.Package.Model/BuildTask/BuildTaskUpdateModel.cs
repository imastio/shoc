using System;

namespace Shoc.Package.Model.BuildTask;

/// <summary>
/// The build task update model
/// </summary>
public class BuildTaskUpdateModel
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
    public string TargetPackageId { get; set; }
    
    /// <summary>
    /// The error code if any
    /// </summary>
    public string ErrorCode { get; set; }
}