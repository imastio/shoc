using System.Collections.Generic;

namespace Shoc.Identity.Model.Privileges;

/// <summary>
/// The privilege access update model.
/// </summary>
public class PrivilegeAccessUpdateModel
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
