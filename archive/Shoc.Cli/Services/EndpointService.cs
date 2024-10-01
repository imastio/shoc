using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shoc.Cli.Model;
using Shoc.ClientCore;

namespace Shoc.Cli.Services
{
    /// <summary>
    /// The endpoint service
    /// </summary>
    public class EndpointService
    {
        /// <summary>
        /// Getting endpoints for profile
        /// </summary>
        /// <param name="profile">The shoc profile</param>
        /// <returns></returns>
        public async Task<DiscoveryEndpoints> GetEndpoints(ShocProfile profile)
        {
            // init http client handler and trust ssl certificate
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };

            // init http client
            using var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(profile.Backend),
            };

            // get endpoints and map to model
            return await (await httpClient.GetAsync("discovery/endpoints")).Map<DiscoveryEndpoints>();
        }
    }
}
