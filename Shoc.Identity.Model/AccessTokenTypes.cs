namespace Shoc.Identity.Model
{
    /// <summary>
    /// The token expiration times
    /// </summary>
    public class AccessTokenTypes
    {
        /// <summary>
        /// The JWT token
        /// </summary>
        public const string JWT = "jwt";

        /// <summary>
        /// The reference token
        /// </summary>
        public const string REFERENCE = "reference";
    }
}