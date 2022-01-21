namespace Shoc.Core.Security
{
    /// <summary>
    /// The password check result
    /// </summary>
    public class PasswordCheckResult
    {
        /// <summary>
        /// Indicates if verification is passed
        /// </summary>
        public bool Verified { get; set; }

        /// <summary>
        /// Checks if password needs upgrade
        /// </summary>
        public bool NeedsUpgrade { get; set; }
    }
}