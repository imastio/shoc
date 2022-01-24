namespace Shoc.Identity.Config
{
    /// <summary>
    /// The sign-on settings
    /// </summary>
    public class SignOnSettings
    {
        /// <summary>
        /// The default no-reply sender
        /// </summary>
        public string NoReplySender { get; set; }

        /// <summary>
        /// Indicates if sign-up is enabled
        /// </summary>
        public bool SignUpEnabled { get; set; }
    }
}
