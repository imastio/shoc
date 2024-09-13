using System;

namespace Shoc.Workspace.Model.Workspace;

/// <summary>
/// The workspace invitation model
/// </summary>
public class WorkspaceInvitationModel
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
    /// The expiration time
    /// </summary>
    public DateTime Expiration { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}