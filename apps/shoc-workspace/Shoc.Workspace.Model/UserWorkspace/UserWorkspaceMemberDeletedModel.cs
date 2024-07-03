namespace Shoc.Workspace.Model.UserWorkspace;

/// <summary>
/// The user workspace member delete result model
/// </summary>
public class UserWorkspaceMemberDeletedModel
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }

    /// <summary>
    /// The id of the associated user
    /// </summary>
    public string UserId { get; set; }
}