namespace Shoc.Identity.Model
{
    /// <summary>
    /// The sign-in principal 
    /// </summary>
    public class SignInPrincipal
    {
        /// <summary>
        /// The subject of sign-in
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The display name of subject
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The email of attempt subject
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The identity provider
        /// </summary>
        public string Provider { get; set; }
    }
}