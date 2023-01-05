using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.Cluster
{
    /// <summary>
    /// The cluster list command handler
    /// </summary>
    public class ClusterListCommandHandler : ShocCommandHandlerBase
    {
        /// <summary>
        /// The client service
        /// </summary>
        protected readonly ClientService clientService;

        /// <summary>
        /// The authentication service
        /// </summary>
        protected readonly AuthService authService;

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ClusterListCommandHandler(ClientService clientService, AuthService authService) 
        {
            this.clientService = clientService;
            this.authService = authService;
        }

        /// <summary>
        /// Implementation of command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // get the clusters in authorized context
            var clusters = await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Builder(profile);

                // load clusters of the project
                return client.GetClusters(status.AccessToken);
            });

            // print header
            Console.WriteLine("Id\t\tName\t\tApi Server");

            // print clusters
            foreach (var cl in clusters)
            {
                Console.WriteLine($"{cl.Id}\t\t{cl.Name}\t\t{cl.ApiServerUri}");
            }

            return 0;
        }
    }
}