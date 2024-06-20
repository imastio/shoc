namespace Shoc.Workspace.Model.Common;

/// <summary>
/// The workspace user reference
/// </summary>
public class WorkspaceUserModel
{
    /// <summary>
    /// The user id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The user email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// The username of the user
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// The type of the user
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The full name of the user
    /// </summary>
    public string FullName { get; set; }
}