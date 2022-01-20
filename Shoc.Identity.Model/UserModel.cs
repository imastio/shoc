using System;

namespace Shoc.Identity.Model
{
    /// <summary>
    /// The identity user model
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// The user id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// If email is verified
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// The username
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// The full name of user
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// The creation time
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The update time
        /// </summary>
        public DateTime Updated { get; set; }
    }
}