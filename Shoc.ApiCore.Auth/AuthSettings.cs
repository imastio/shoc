namespace Shoc.ApiCore.Auth
{
    /// <summary>
    /// The authentication settings for the API
    /// </summary>
    public class AuthSettings
    {
        /// <summary>
        /// The identity provider
        /// </summary>
        public string Authority { get; set; }
        
        /// <summary>
        /// The audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Allow self-signed certificate and http connections
        /// </summary>
        public bool AllowInsecure { get; set; }
    }
}