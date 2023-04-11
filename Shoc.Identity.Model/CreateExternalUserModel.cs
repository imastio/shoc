namespace Shoc.Identity.Model
{
    /// <summary>
    /// Create External User Model entity
    /// </summary>
    public class CreateExternalUserModel
    {
        /// <summary>
        /// The external user id
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// The user id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The external provider name
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// The email address
        /// </summary>
        public string Email { get; set; }
    }
}