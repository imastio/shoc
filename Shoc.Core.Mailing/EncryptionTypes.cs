namespace Shoc.Core.Mailing
{
    /// <summary>
    /// The types of mailing encryption
    /// </summary>
    public static class EncryptionTypes
    {
        /// <summary>
        /// No encryption
        /// </summary>
        public const string NONE = "none";

        /// <summary>
        /// The auto type of choice
        /// </summary>
        public const string AUTO = "auto";

        /// <summary>
        /// The SSL type of encryption
        /// </summary>
        public const string SSL = "ssl";

        /// <summary>
        /// The start tls
        /// </summary>
        public const string START_TLS = "start_tls";

        /// <summary>
        /// The start TLS when available
        /// </summary>
        public const string START_TLS_WHEN_AVAILABLE = "start_tls_when_available";
    }
}