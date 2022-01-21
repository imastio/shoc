namespace Shoc.Identity.Model
{
    /// <summary>
    /// Create User Model entity
    /// </summary>
    public class CreateUserModel
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
        /// The given password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The password hash
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// The first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The full name of user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The role to assign
        /// </summary>
        public string Role { get; set; }
    }
}