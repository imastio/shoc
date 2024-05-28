namespace Shoc.Mailing.Model;

/// <summary>
/// The input to update profile API Secret
/// </summary>
public class UpdateProfileApiSecretInput
{
    /// <summary>
    /// The id of profile
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The plain text API Secret to update
    /// </summary>
    public string ApiSecret { get; set; }

    /// <summary>
    /// The encrypted API Secret to update
    /// </summary>
    public string ApiSecretEncrypted { get; set; }
}
