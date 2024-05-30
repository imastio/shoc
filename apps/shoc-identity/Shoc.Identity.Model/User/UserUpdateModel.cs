namespace Shoc.Identity.Model.User
{
    /// <summary>
    /// The user update model
    /// </summary>
    public class UserUpdateModel
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
        /// The full name of user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The user timezone
        /// </summary>
        public string Timezone { get; set; }
    }
}