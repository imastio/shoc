namespace Shoc.Job.K8s.Model;

/// <summary>
/// The default metadata for kubernetes objects
/// </summary>
public class ManagedMetadata
{
    /// <summary>
    /// The object name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The object instance
    /// </summary>
    public string Instance { get; set; }
    
    /// <summary>
    /// The object component
    /// </summary>
    public string Component { get; set; }
    
    /// <summary>
    /// The containing component
    /// </summary>
    public string PartOf { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The job id
    /// </summary>
    public string JobId { get; set; }
    
    /// <summary>
    /// The task id
    /// </summary>
    public string TaskId { get; set; }
}