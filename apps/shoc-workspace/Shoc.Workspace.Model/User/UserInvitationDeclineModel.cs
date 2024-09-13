namespace Shoc.Workspace.Model.User;

/// <summary>
/// The user invitation decline model
/// </summary>
public class UserInvitationDeclineModel
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