namespace Shoc.Identity.Model.User;

/// <summary>
/// The user access update result model.
/// </summary>
public class UserAccessUpdateResultModel
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
