using System;

namespace Shoc.Workspace.Model.User;

/// <summary>
/// The user invitation model
/// </summary>
public class UserInvitationModel
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
    /// The email of invitee
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// The invitation role
    /// </summary>
    public string Role { get; set; }
    
    /// <summary>
    /// The inviting user
    /// </summary>
    public string InvitedBy { get; set; }
    
    /// <summary>
    /// The inviting user email
    /// </summary>
    public string InvitedByEmail { get; set; }
    
    /// <summary>
    /// The inviting user full name
    /// </summary>
    public string InvitedByFullName { get; set; }
    
    /// <summary>
    /// The expiration time
    /// </summary>
    public DateTime Expiration { get; set; }

    /// <summary>
    /// The name of workspace
    /// </summary>
    public string WorkspaceName { get; set; }
    
    /// <summary>
    /// The description of workspace
    /// </summary>
    public string WorkspaceDescription { get; set; }
    
    /// <summary>
    /// The type of workspace
    /// </summary>
    public string WorkspaceType { get; set; }

}