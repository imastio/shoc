namespace Shoc.Identity.Model.User;

/// <summary>
/// The referential value model for the user
/// </summary>
public class UserReferentialValueModel
{
    /// <summary>
    /// The id of the user
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The full name of the user
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// The email of the user
    /// </summary>
    public string Email { get; set; }
}