namespace Shoc.Identity.OpenId
{
    /// <summary>
    /// The native client settings
    /// </summary>
    public class NativeClientSettings
    {
        /// <summary>
        /// The client id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The access token expiration in seconds
        /// </summary>
        public int? AccessTokenExpiration { get; set; }

        /// <summary>
        /// The refresh token expiration
        /// </summary>
        public int? RefreshTokenExpiration { get; set; }
    }
}