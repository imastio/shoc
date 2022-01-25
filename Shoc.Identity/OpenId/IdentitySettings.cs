namespace Shoc.Identity.OpenId
{
    /// <summary>
    /// The identity settings
    /// </summary>
    public class IdentitySettings
    {
        /// <summary>
        /// The public origin of the identity server
        /// </summary>
        public string PublicOrigin { get; set; }

        /// <summary>
        /// The base path for the identity provider
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// The issuer uri
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The signing key path
        /// </summary>
        public string SigningKeyPath { get; set; }

        /// <summary>
        /// The login url
        /// </summary>
        public string SignInUrl { get; set; }

        /// <summary>
        /// The login url
        /// </summary>
        public string SignOutUrl { get; set; }

        /// <summary>
        /// The error url
        /// </summary>
        public string ErrorUrl { get; set; }

        /// <summary>
        /// The machine to machine settings
        /// </summary>
        public MachineClientSettings MachineClient { get; set; }

        /// <summary>
        /// The interactive client settings
        /// </summary>
        public InteractiveClientSettings InteractiveClient { get; set; }

        /// <summary>
        /// The native client settings
        /// </summary>
        public NativeClientSettings NativeClient { get; set; }
    }
}