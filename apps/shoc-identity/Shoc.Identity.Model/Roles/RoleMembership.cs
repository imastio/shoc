namespace Shoc.Identity.Model.Roles;

/// <summary>
/// The role membership.
/// </summary>
public class RoleMembership
{
    /// <summary>
    /// The role identifier.
    /// </summary>
    public string RoleId { get; set; }

    /// <summary>
    /// The user identifier.
    /// </summary>
    public string UserId { get; set; }
}
