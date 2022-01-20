namespace Shoc.Identity.Model
{
    /// <summary>
    /// The token expiration times
    /// </summary>
    public class TokenExpirationTypes
    {
        /// <summary>
        /// The sliding token expiration
        /// </summary>
        public const string SLIDING = "sliding";

        /// <summary>
        /// The absolute expiration
        /// </summary>
        public const string ABSOLUTE = "absolute";
    }
}