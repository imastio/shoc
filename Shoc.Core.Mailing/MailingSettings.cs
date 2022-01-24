namespace Shoc.Core.Mailing
{
    /// <summary>
    /// The mailing settings
    /// </summary>
    public class MailingSettings
    {
        /// <summary>
        /// The SMTP server
        /// </summary>
        public string Server { get; set; }
        
        /// <summary>
        /// The SMTP port
        /// </summary>
        public int Port { get; set; }
        
        /// <summary>
        /// The relay login
        /// </summary>
        public string Login { get; set; }
        
        /// <summary>
        /// The relay password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The encryption type
        /// </summary>
        public string EncryptionType { get; set; }

        /// <summary>
        /// The default from email
        /// </summary>
        public string DefaultFromEmail{ get; set; }

        /// <summary>
        /// The default from sender
        /// </summary>
        public string DefaultFromSender { get; set; }
    }
}