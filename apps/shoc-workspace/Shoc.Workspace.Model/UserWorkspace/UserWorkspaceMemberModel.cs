using System;

namespace Shoc.Workspace.Model.UserWorkspace;

/// <summary>
/// The user workspace member
/// </summary>
public class UserWorkspaceMemberModel
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
    /// The user id
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The email of the member
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// The full name of the member
    /// </summary>
    public string FullName { get; set; }
    
    /// <summary>
    /// The membership role
    /// </summary>
    public string Role { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}