namespace Shoc.Engine.Client
{
    /// <summary>
    /// The engine client settings
    /// </summary>
    public class EngineClientSettings
    {
        /// <summary>
        /// The address of container engine 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The type of container engine
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The flag to indicate if TLS should be verified
        /// </summary>
        public bool TlsVerify { get; set; }

        /// <summary>
        /// The certificate path
        /// </summary>
        public string CertPath { get; set; }

        /// <summary>
        /// The version of engine
        /// </summary>
        public string Version { get; set; }
    }
}