namespace Shoc.Identity.Model.Privileges;

/// <summary>
/// The privilege update model.
/// </summary>
public class PrivilegeUpdateModel
{
    /// <summary>
    /// The privilege id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The privilege name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The privilege description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// The privilege category.
    /// </summary>
    public string Category { get; set; }
}
