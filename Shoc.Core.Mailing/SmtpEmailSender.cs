using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Shoc.Core.Mailing
{
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
        /// Creates new instance of SMTP-based email sender
        /// </summary>
        /// <param name="settings">The settings</param>
        public SmtpEmailSender(MailingSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Sends the email message
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns></returns>
        public async Task<EmailResult> SendAsync(EmailMessage message)
        {
            try
            {
                return await this.SendImplAsync(message);
            }
            catch (Exception e)
            {
                throw new ShocException(new List<ErrorDefinition> { ErrorDefinition.Unknown("MAILING_ERROR") }, "Could not send an email", e);
            }
        }
        
        /// <summary>
        /// Sends the email message
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns></returns>
        private async Task<EmailResult> SendImplAsync(EmailMessage message)
        {
            // create email message
            var email = new MimeMessage();

            // use from email or default
            var fromEmail = message.FromEmail ?? this.settings.DefaultFromEmail;

            // use from sender or default
            var fromSender = message.FromSender ?? this.settings.DefaultFromSender; 

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

            // create new client
            using var smtp = new SmtpClient();
            
            // connect to server
            await smtp.ConnectAsync(this.settings.Server, this.settings.Port, ResolveEncryption(this.settings.EncryptionType));

            // authenticate in server
            await smtp.AuthenticateAsync(this.settings.Login, this.settings.Password);
            
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
}