using System.Collections.Generic;

namespace Shoc.Identity.Model.User;

/// <summary>
/// The user access update model.
/// </summary>
public class UserAccessUpdateModel
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
