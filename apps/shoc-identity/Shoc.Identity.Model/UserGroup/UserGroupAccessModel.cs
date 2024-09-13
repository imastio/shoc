namespace Shoc.Identity.Model.UserGroup;

/// <summary>
/// The user group access model.
/// </summary>
public class UserGroupAccessModel
{
    /// <summary>
    /// The access grant id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The group identifier.
    /// </summary>
    public string GroupId { get; set; }

    /// <summary>
    /// The access modifier.
    /// </summary>
    public string Access { get; set; }
}
