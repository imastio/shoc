using System;

namespace Shoc.Cli.Model
{
    /// <summary>
    /// The auth status model
    /// </summary>
    public class AuthStatus
    {
        /// <summary>
        /// The user id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The name of user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The email address of user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's username 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The session expiration
        /// </summary>
        public DateTime SessionExpiration { get; set; }

        /// <summary>
        /// The reference to access token
        /// </summary>
        public string AccessToken { get; set; }
    }
}