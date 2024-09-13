using System;

namespace Shoc.Identity.Model.Roles;

/// <summary>
/// The role model.
/// </summary>
public class RoleModel
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

    /// <summary>
    /// The role creation time.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// The role update time.
    /// </summary>
    public DateTime Updated { get; set; }
}
