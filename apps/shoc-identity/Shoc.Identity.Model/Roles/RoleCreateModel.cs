namespace Shoc.Identity.Model.Roles;

/// <summary>
/// The role creation model.
/// </summary>
public class RoleCreateModel
{
    /// <summary>
    /// The role id.
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
