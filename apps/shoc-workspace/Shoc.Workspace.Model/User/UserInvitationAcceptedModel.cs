namespace Shoc.Workspace.Model.User;

/// <summary>
/// The user invitation accepted model
/// </summary>
public class UserInvitationAcceptedModel
{
    /// <summary>
    /// The invitation id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The membership record id
    /// </summary>
    public string MembershipId { get; set; }
}