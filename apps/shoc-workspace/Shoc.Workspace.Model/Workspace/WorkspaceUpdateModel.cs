namespace Shoc.Workspace.Model.Workspace;

/// <summary>
/// The workspace update model
/// </summary>
public class WorkspaceUpdateModel
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The workspace description
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// The workspace status
    /// </summary>
    public string Status { get; set; }
}