namespace Shoc.Mailing.Model;

/// <summary>
/// The mailing profile creation input
/// </summary>
public class CreateMailingProfileInput
{
    /// <summary>
    /// The identifier
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The unique profile code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// The type of provider
    /// </summary>
    public string Provider { get; set; }

    /// <summary>
    /// The SMTP server
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// The SMTP port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// The SMTP username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The plain text password
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The encrypted password
    /// </summary>
    public string PasswordEncrypted { get; set; }

    /// <summary>
    /// The encryption type
    /// </summary>
    public string EncryptionType { get; set; }

    /// <summary>
    /// The service url in case of API level integration
    /// </summary>
    public string ApiUrl { get; set; }

    /// <summary>
    /// The application identifier
    /// </summary>
    public string ApplicationId { get; set; }

    /// <summary>
    /// The plain text API secret key
    /// </summary>
    public string ApiSecret { get; set; }

    /// <summary>
    /// The encrypted API secret key
    /// </summary>
    public string ApiSecretEncrypted { get; set; }

    /// <summary>
    /// The default FROM email
    /// </summary>
    public string DefaultFromEmail { get; set; }

    /// <summary>
    /// The default FROM sender name
    /// </summary>
    public string DefaultFromSender { get; set; }
}

