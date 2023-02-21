using Imast.Ext.DiscoveryCore;
using Shoc.ClientCore;

namespace Shoc.Identity.Client
{
    /// <summary>
    /// The identity api client
    /// </summary>
    public class IdentityClient : ShocApiClient
    {
        /// <summary>
        /// The service name by default
        /// </summary>
        private const string DEFAULT_SERVICE = "shoc-identity";

        /// <summary>
        /// Creates new instance of the client
        /// </summary>
        /// <param name="client">The client name</param>
        /// <param name="service">The service</param>
        /// <param name="discovery">The discovery</param>
        public IdentityClient(string client, string service, IDiscoveryClient discovery) : base(client, service ?? DEFAULT_SERVICE, discovery)
        {
        }

        /// <summary>
        /// Creates new instance of the client
        /// </summary>
        /// <param name="client">The client name</param>
        /// <param name="discovery">The discovery</param>
        public IdentityClient(string client, IDiscoveryClient discovery) : base(client, DEFAULT_SERVICE, discovery)
        {
        }
    }
}