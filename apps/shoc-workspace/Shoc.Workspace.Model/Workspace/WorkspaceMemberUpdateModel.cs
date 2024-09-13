namespace Shoc.Workspace.Model.Workspace;

/// <summary>
/// The workspace member update model
/// </summary>
public class WorkspaceMemberUpdateModel
{
    /// <summary>
    /// The membership id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The membership role
    /// </summary>
    public string Role { get; set; }
}