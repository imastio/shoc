namespace Shoc.Identity.Model
{
    /// <summary>
    /// The sign-up model
    /// </summary>
    public class SignUpFlowInput
    {
        /// <summary>
        /// The preferred email for sign-up
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The preferred password for sign-up
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The full name for sign-up
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The return url
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}