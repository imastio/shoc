using System.Threading.Tasks;

namespace Shoc.Core.Mailing
{
    /// <summary>
    /// The email sender interface
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends the email message
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns></returns>
        Task<EmailResult> SendAsync(EmailMessage message);
    }
}