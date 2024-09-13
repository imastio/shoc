using System;

namespace Shoc.Workspace.Model.UserWorkspace;

/// <summary>
/// The user workspace model
/// </summary>
public class UserWorkspaceModel
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// The id of the associated user
    /// </summary>
    public string UserId { get; set; }
    
    /// <summary>
    /// The role of the associated user
    /// </summary>
    public string Role { get; set; }
    
    /// <summary>
    /// The unique name of workspace
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The workspace description
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// The workspace type
    /// </summary>
    public string Type { get; set; }
    
    /// <summary>
    /// The workspace status
    /// </summary>
    public string Status { get; set; }
    
    /// <summary>
    /// The creation time
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// The update time
    /// </summary>
    public DateTime Updated { get; set; }
}