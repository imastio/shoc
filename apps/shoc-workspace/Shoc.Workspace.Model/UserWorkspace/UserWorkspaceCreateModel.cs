namespace Shoc.Workspace.Model.UserWorkspace;

/// <summary>
/// The user workspace create model
/// </summary>
public class UserWorkspaceCreateModel
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
}