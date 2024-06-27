namespace Shoc.Workspace.Model.Workspace;

/// <summary>
/// The workspace create model
/// </summary>
public class WorkspaceCreateModel
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The unique name of workspace
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The workspace description
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// The workspace type
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The workspace status
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The creating user
    /// </summary>
    public string CreatedBy { get; set; }
}