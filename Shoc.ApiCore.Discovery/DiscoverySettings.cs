namespace Shoc.ApiCore.Discovery
{
    /// <summary>
    /// The discovery settings
    /// </summary>
    public class DiscoverySettings
    {
        /// <summary>
        /// The primary type of discovery
        /// </summary>
        public string Primary { get; set; }

        /// <summary>
        /// The fallback discovery
        /// </summary>
        public string Fallback { get; set; }
    }
}