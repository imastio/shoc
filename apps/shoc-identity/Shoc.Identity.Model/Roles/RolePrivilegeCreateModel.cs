namespace Shoc.Identity.Model.Roles;

/// <summary>
/// The role privilege create model.
/// </summary>
public class RolePrivilegeCreateModel
{
    /// <summary>
    /// The role identifier.
    /// </summary>
    public string RoleId { get; set; }

    /// <summary>
    /// The privilege identifier.
    /// </summary>
    public string PrivilegeId { get; set; }
}
