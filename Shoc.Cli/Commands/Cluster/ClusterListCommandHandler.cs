using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleTables;
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
                var client = this.clientService.Executor(profile);

                // load clusters of the project
                return client.GetClusters(status.AccessToken);
            });

            // add headers
            var table = new ConsoleTable("Id", "Name", "Api Server");

            // print clusters
            foreach (var cl in clusters)
            {
                table.AddRow(cl.Id, cl.Name, cl.ApiServerUri);
            }

            // print table
            table.Write();

            return 0;
        }
    }
}