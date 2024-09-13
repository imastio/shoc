namespace Shoc.Workspace.Model.User;

/// <summary>
/// The user invitation accept model
/// </summary>
public class UserInvitationAcceptModel
{
    /// <summary>
    /// The invitation id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
}