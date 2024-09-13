namespace Shoc.Workspace.Model.UserWorkspace;

/// <summary>
/// The user workspace update result model
/// </summary>
public class UserWorkspaceUpdatedModel
{
    /// <summary>
    /// The workspace id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The id of the associated user
    /// </summary>
    public string UserId { get; set; }
}