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
        /// Gets the discovery for the profile
        /// </summary>
        /// <param name="profile">The profile</param>
        /// <returns></returns>
        public IDiscoveryClient GetDiscovery(ShocProfile profile)
        {
            return new GatewayDiscoveryClient(profile.Api);
        }
    }
}