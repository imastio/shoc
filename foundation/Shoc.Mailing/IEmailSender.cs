using System.Threading.Tasks;
using Shoc.Mailing.Model;

namespace Shoc.Mailing;

/// <summary>
/// The email sender interface
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends the email message
    /// </summary>
    /// <param name="message">The message to send</param>
    /// <param name="profile">The mailing profile code</param>
    /// <returns></returns>
    Task<EmailResult> SendAsync(EmailMessage message, string profile = null);
}