using System.Collections.Generic;

namespace Shoc.Identity.Model.UserGroup;

/// <summary>
/// The user group access update model.
/// </summary>
public class UserGroupAccessUpdateModel
{
    /// <summary>
    /// The access modifiers to grant.
    /// </summary>
    public List<string> Grant { get; set; }

    /// <summary>
    /// The access modifiers to revoke.
    /// </summary>
    public List<string> Revoke { get; set; }
}
