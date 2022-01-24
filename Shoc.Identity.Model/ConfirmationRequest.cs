namespace Shoc.Identity.Model
{
    /// <summary>
    /// The confirmation request
    /// </summary>
    public class ConfirmationRequest
    {
        /// <summary>
        /// The email to verify
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The return url
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}