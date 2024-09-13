using System;

namespace Shoc.Workspace.Model.Workspace;

/// <summary>
/// The workspace invitation update model
/// </summary>
public class WorkspaceInvitationUpdateModel
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
    /// The invitation role
    /// </summary>
    public string Role { get; set; }
    
    /// <summary>
    /// The expiration time
    /// </summary>
    public DateTime? Expiration { get; set; }
}