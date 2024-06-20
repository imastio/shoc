using System;

namespace Shoc.Workspace.Model.Workspace;

/// <summary>
/// The workspace member extended model
/// </summary>
public class WorkspaceMemberExtendedModel
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