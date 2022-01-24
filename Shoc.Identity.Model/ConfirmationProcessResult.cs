namespace Shoc.Identity.Model
{
    /// <summary>
    /// The result of confirmation process
    /// </summary>
    public class ConfirmationProcessResult
    {
        /// <summary>
        /// The subject of authorization
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The return url
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// The flow continues
        /// </summary>
        public bool ContinueFlow { get; set; }
    }
}