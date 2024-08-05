namespace Shoc.Workspace.Model.UserWorkspace;

/// <summary>
/// The user workspace invitation update result model
/// </summary>
public class UserWorkspaceInvitationUpdateModel
{
    /// <summary>
    /// The workspace member id
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