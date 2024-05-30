namespace Shoc.Identity.Model.UserGroup;

/// <summary>
/// The user group access update result model.
/// </summary>
public class UserGroupAccessUpdateResultModel
{
    /// <summary>
    /// The number of granted accesses.
    /// </summary>
    public int Granted { get; set; }

    /// <summary>
    /// The number of revoked accesses.
    /// </summary>
    public int Revoked { get; set; }
}
