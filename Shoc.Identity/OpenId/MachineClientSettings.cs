namespace Shoc.Identity.OpenId
{
    /// <summary>
    /// The machine to machine client settings
    /// </summary>
    public class MachineClientSettings
    {
        /// <summary>
        /// The client id for m2m communication
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The secret for the given client
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// The access token expiration in seconds
        /// </summary>
        public int? AccessTokenExpiration { get; set; }
    }
}