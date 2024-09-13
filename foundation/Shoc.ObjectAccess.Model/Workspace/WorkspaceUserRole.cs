namespace Shoc.ObjectAccess.Model.Workspace;

/// <summary>
/// Workspace user role
/// </summary>
public class WorkspaceUserRole
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string WorkspaceId { get; set; }
    
    /// <summary>
    /// The user id 
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The workspace role
    /// </summary>
    public string Role { get; set; }
}