using System;

namespace Shoc.Identity.Model
{
    /// <summary>
    /// The confirmation code record
    /// </summary>
    public class ConfirmationCode
    {
        /// <summary>
        /// The record id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The user identifier
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// The email address 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///  The hash of confirmation code
        /// </summary>
        public string CodeHash { get; set; }
        
        /// <summary>
        /// The confirmation link
        /// </summary>
        public string Link { get; set; }
        
        /// <summary>
        /// The code is valid until
        /// </summary>
        public DateTime ValidUntil { get; set; }

        /// <summary>
        /// The return url if context is available
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// The creation time
        /// </summary>
        public DateTime Created { get; set; }
    }
}