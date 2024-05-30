namespace Shoc.Identity.Model.Privileges;

/// <summary>
/// The privilege access model.
/// </summary>
public class PrivilegeAccessModel
{
    /// <summary>
    /// The privilege access grant id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The privilege identifier,
    /// </summary>
    public string PrivilegeId { get; set; }

    /// <summary>
    /// The access modifier.
    /// </summary>
    public string Access { get; set; }
}
