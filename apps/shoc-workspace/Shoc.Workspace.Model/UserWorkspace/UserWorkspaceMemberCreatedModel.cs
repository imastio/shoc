namespace Shoc.Workspace.Model.UserWorkspace;

/// <summary>
/// The user workspace invitation created result model
/// </summary>
public class UserWorkspaceInvitationCreatedModel
{
    /// <summary>
    /// The workspace invitation id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }

    /// <summary>
    /// The email address to invite
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// The membership role
    /// </summary>
    public string Role { get; set; }
}