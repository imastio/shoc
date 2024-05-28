using System;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.DataProtection;
using MimeKit;
using Shoc.Core;
using Shoc.Mailing.Model;

namespace Shoc.Mailing.Smtp;

/// <summary>
/// The SMTP email sender
/// </summary>
public class SmtpEmailSender : IEmailSender
{
    /// <summary>
    /// The mailing settings
    /// </summary>
    private readonly MailingSettings settings;

    /// <summary>
    /// The profile repository
    /// </summary>
    private readonly IMailingProfileRepository profileRepository;

    /// <summary>
    /// The data protection provider
    /// </summary>
    private readonly IDataProtectionProvider protectionProvider;

    /// <summary>
    /// Creates new instance of SMTP-based email sender
    /// </summary>
    /// <param name="settings">The settings</param>
    /// <param name="profileRepository">The profile repository</param>
    /// <param name="protectionProvider">The data protection provider</param>
    public SmtpEmailSender(MailingSettings settings, IMailingProfileRepository profileRepository, IDataProtectionProvider protectionProvider)
    {
        this.settings = settings;
        this.profileRepository = profileRepository;
        this.protectionProvider = protectionProvider;
    }

    /// <summary>
    /// Sends the email message
    /// </summary>
    /// <param name="message">The message to send</param>
    /// <param name="profile">The profile name</param>
    /// <returns></returns>
    public async Task<EmailResult> SendAsync(EmailMessage message, string profile = null)
    {
        // try load the profile
        var result = await this.profileRepository.GetByCode(profile ?? this.settings.DefaultProfile);

        // missing email profile
        if (result == null)
        {
            throw ErrorDefinition.Validation(MailingErrors.MISSING_PROFILE).AsException();
        }

        // the provider is not SMTP
        if (!string.Equals(result.Provider, MailProviders.SMTP))
        {
            throw ErrorDefinition.Validation(MailingErrors.INVALID_PROVIDER).AsException();
        }
        
        // make sure password exists
        if (string.IsNullOrWhiteSpace(result.PasswordEncrypted))
        {
            throw ErrorDefinition.Validation(MailingErrors.MISSING_PASSWORD).AsException();
        }

        try
        {
            return await SendImplAsync(result, message);
        }
        catch (AuthenticationException e)
        {
            throw ErrorDefinition.Validation(MailingErrors.INVALID_CREDENTIALS, e.Message).AsException();
        }
        catch (Exception e)
        {
            throw ErrorDefinition.Unknown(MailingErrors.UNKNOWN_ERROR, e.Message).AsException();
        }
    }

    /// <summary>
    /// Sends the email message
    /// </summary>
    /// <param name="profile">The email sending profile</param>
    /// <param name="message">The message to send</param>
    /// <returns></returns>
    private async Task<EmailResult> SendImplAsync(MailingProfile profile, EmailMessage message)
    {
        // create email message
        var email = new MimeMessage();

        // use from email or default
        var fromEmail = message.FromEmail ?? profile.DefaultFromEmail;

        // use from sender or default
        var fromSender = message.FromSender ?? profile.DefaultFromSender; 

        // add from
        email.Sender = new MailboxAddress(fromSender, fromEmail);
        
        // add sender to from
        email.From.Add(email.Sender);

        // add to
        email.To.AddRange(message.To?.Select(MailboxAddress.Parse) ?? Enumerable.Empty<MailboxAddress>());
        
        // add cc
        email.Cc.AddRange(message.Cc?.Select(MailboxAddress.Parse) ?? Enumerable.Empty<MailboxAddress>());

        // add bcc
        email.Bcc.AddRange(message.Bcc?.Select(MailboxAddress.Parse) ?? Enumerable.Empty<MailboxAddress>());

        // add subject
        email.Subject = message.Subject;

        // create body builder
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.Body
        };
        
        // do for every linked resource
        message.Resources?.ForEach(resource =>
        {
            // add a resource
            var entity = (MimePart) bodyBuilder.LinkedResources.Add(resource.Path, ContentType.Parse(resource.Type));

            // assign an id for referencing
            entity.ContentId = resource.Id;

            // transfer as base64
            entity.ContentTransferEncoding = ContentEncoding.Base64;
        });

        // set body
        email.Body = bodyBuilder.ToMessageBody();

        // create protector
        var protector = this.protectionProvider.CreateProtector(MailingProtectionConstants.MAILING_CREDENTIALS_PURPOSE);

        // unprotect the password to use
        var password = protector.Unprotect(profile.PasswordEncrypted);
        
        // create new client
        using var smtp = new SmtpClient();
        
        // connect to server
        await smtp.ConnectAsync(profile.Server, profile.Port, ResolveEncryption(profile.EncryptionType));

        // authenticate in server
        await smtp.AuthenticateAsync(profile.Username, password);
        
        // try send an email
        await smtp.SendAsync(email);

        // try quietly disconnect
        await smtp.DisconnectAsync(true);
        
        // the email is sent with success
        return new EmailResult
        {
            Sent = true
        };
    }

    /// <summary>
    /// Resolves encryption type based on settings
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns></returns>
    private static SecureSocketOptions ResolveEncryption(string type)
    {
        return type switch
        {
            EncryptionTypes.NONE => SecureSocketOptions.None,
            EncryptionTypes.AUTO => SecureSocketOptions.Auto,
            EncryptionTypes.SSL => SecureSocketOptions.SslOnConnect,
            EncryptionTypes.START_TLS => SecureSocketOptions.StartTls,
            EncryptionTypes.START_TLS_WHEN_AVAILABLE => SecureSocketOptions.StartTlsWhenAvailable,
            _ => SecureSocketOptions.None
        };
    }
}
