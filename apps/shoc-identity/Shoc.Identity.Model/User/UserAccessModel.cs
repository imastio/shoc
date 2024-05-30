namespace Shoc.Identity.Model.User;

/// <summary>
/// The user access model.
/// </summary>
public class UserAccessModel
{
    /// <summary>
    /// The access grant id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The user identifier
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// The access modifier
    /// </summary>
    public string Access { get; set; }
}
