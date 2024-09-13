namespace Shoc.Identity.Model.Roles;

/// <summary>
/// The role update model.
/// </summary>
public class RoleUpdateModel
{
    /// <summary>
    /// The role identifier.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The role name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The role description.
    /// </summary>
    public string Description { get; set; }
}
