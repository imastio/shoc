using System;

namespace Shoc.Identity.Model
{
    /// <summary>
    /// The sign-in metadata
    /// </summary>
    public class SignInMetadata
    {
        /// <summary>
        /// The IP address of principal
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// The user agent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// The session id
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// The sign-in time 
        /// </summary>
        public DateTime Time { get; set; }
    }
}