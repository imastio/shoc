using System;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Cli.Services;
using Shoc.Core;

namespace Shoc.Cli.Commands.Cluster
{
    /// <summary>
    /// The cluster delete command handler
    /// </summary>
    public class ClusterDeleteCommandHandler : ShocCommandHandlerBase
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
        /// The given cluster name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ClusterDeleteCommandHandler(ClientService clientService, AuthService authService) 
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
            // make sure name is given
            if (string.IsNullOrEmpty(this.Name))
            {
                throw ErrorDefinition.Validation(CliErrors.CLUSTER_NAME_ERROR).AsException();
            }

            // get cluster by name in authorized context
            var cluster = (await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Executor(profile);

                // load cluster by name
                return client.GetClusters(status.AccessToken, this.Name);
            })).FirstOrDefault();

            // delete cluster by id in authorized context
            cluster = (await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Executor(profile);

                // delete the cluster by id
                return client.DeleteCluster(status.AccessToken, cluster?.Id);
            }));

            Console.WriteLine($"Deleted Kubernetes Cluster id: {cluster.Id}");

            return 0;
        }
    }
}