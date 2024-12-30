namespace Shoc.Job.Model.WorkspaceJob;

/// <summary>
/// The workspace job created model
/// </summary>
public class WorkspaceJobCreatedModel
{
    /// <summary>
    /// The id of the job
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The local id of the job
    /// </summary>
    public long LocalId { get; set; }
}