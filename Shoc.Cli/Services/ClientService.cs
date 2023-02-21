using Shoc.Builder.Client;
using Shoc.Cli.Model;
using Shoc.Executor.Client;

namespace Shoc.Cli.Services
{
    /// <summary>
    /// The client service
    /// </summary>
    public class ClientService
    {
        /// <summary>
        /// The default client name
        /// </summary>
        private const string DEFAULT_CLIENT = "cli";

        /// <summary>
        /// The discovery service
        /// </summary>
        private readonly DiscoveryService discoveryService;

        /// <summary>
        /// Creates new instance of client service
        /// </summary>
        /// <param name="discoveryService">The discovery service</param>
        public ClientService(DiscoveryService discoveryService)
        {
            this.discoveryService = discoveryService;
        }

        /// <summary>
        /// Provide the builder client
        /// </summary>
        /// <returns></returns>
        public BuilderClient Builder(ShocProfile profile)
        {
            return new BuilderClient(DEFAULT_CLIENT, this.discoveryService.GetDiscovery(profile));
        }

        /// <summary>
        /// Provide the executor client
        /// </summary>
        /// <returns></returns>
        public ExecutorClient Executor(ShocProfile profile)
        {
            return new ExecutorClient(DEFAULT_CLIENT, this.discoveryService.GetDiscovery(profile));
        }
    }
}