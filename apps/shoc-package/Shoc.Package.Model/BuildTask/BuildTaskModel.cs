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
    /// The build task type
    /// </summary>
    public string Provider { get; set; }
    
    /// <summary>
    /// The status of the build task
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The build manifest (template, spec, etc.)
    /// </summary>
    public string Manifest { get; set; }
    
    /// <summary>
    /// The deadline of the current status
    /// </summary>
    public DateTime? Deadline { get; set; }
    
    /// <summary>
    /// The last activity
    /// </summary>
    public string LastActivity { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}