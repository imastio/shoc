namespace Shoc.Identity.Model.Privileges;

/// <summary>
/// The privilege access update result model.
/// </summary>
public class PrivilegeAccessUpdateResultModel
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
