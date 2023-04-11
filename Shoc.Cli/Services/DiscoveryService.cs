using Imast.Ext.DiscoveryCore;
using Shoc.Cli.Model;

namespace Shoc.Cli.Services
{
    /// <summary>
    /// The discovery service 
    /// </summary>
    public class DiscoveryService
    {
        /// <summary>
        /// The endpoint service
        /// </summary>
        private readonly EndpointService endpointService;

        /// <summary>
        /// Creates new instance of discovery service
        /// </summary>
        /// <param name="endpointService">The endpoint service</param>
        public DiscoveryService(EndpointService endpointService)
        {
            this.endpointService = endpointService;
        }

        /// <summary>
        /// Gets the discovery for the profile
        /// </summary>
        /// <param name="profile">The profile</param>
        /// <returns></returns>
        public virtual IDiscoveryClient GetDiscovery(ShocProfile profile)
        {
            return new IndirectGatewayDiscoveryClient(profile, endpointService);
        }
    }
}