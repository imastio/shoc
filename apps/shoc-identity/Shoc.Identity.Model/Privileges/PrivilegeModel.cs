using System;

namespace Shoc.Identity.Model.Privileges;

/// <summary>
/// The representation of privilege model.
/// </summary>
public class PrivilegeModel
{
    /// <summary>
    /// The privilege identifier.
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

    /// <summary>
    /// The privilege creation time.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// The privilege update time.
    /// </summary>
    public DateTime Updated { get; set; }
}

