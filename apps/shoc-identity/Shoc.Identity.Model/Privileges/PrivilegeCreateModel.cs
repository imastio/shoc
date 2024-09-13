namespace Shoc.Identity.Model.Privileges;

/// <summary>
/// The privilege create model.
/// </summary>
public class PrivilegeCreateModel
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

