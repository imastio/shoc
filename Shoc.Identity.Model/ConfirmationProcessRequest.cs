namespace Shoc.Identity.Model
{
    /// <summary>
    /// The confirmation process request
    /// </summary>
    public class ConfirmationProcessRequest
    {
        /// <summary>
        /// The email to confirm
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The confirmation code
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// The return url
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}