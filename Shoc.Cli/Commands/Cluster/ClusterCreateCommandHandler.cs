using System;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using Shoc.Builder.Model.Kubernetes;
using Shoc.Cli.Services;
using Shoc.Core;

namespace Shoc.Cli.Commands.Cluster
{
    /// <summary>
    /// The cluster create command handler
    /// </summary>
    public class ClusterCreateCommandHandler : ShocCommandHandlerBase
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
        /// The given kubeconfig path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ClusterCreateCommandHandler(ClientService clientService, AuthService authService) 
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
            // make sure the path is given and exists
            if (string.IsNullOrWhiteSpace(this.Path) || !File.Exists(this.Path))
            {
                throw ErrorDefinition.Validation(CliErrors.CLUSTER_KUBECONFIG_PATH_ERROR).AsException();
            }

            // get file content
            var contents = await File.ReadAllTextAsync(this.Path);

            // make sure kubeconfig not empty
            if (string.IsNullOrWhiteSpace(contents))
            {
                throw ErrorDefinition.Validation(CliErrors.CLUSTER_KUBECONFIG_EMPTY_ERROR).AsException();
            }

            // enter name for docker registry
            Console.Write("Name: ");
            var name = Console.ReadLine();

            // check name not empty
            if (string.IsNullOrEmpty(name))
            {
                throw ErrorDefinition.Validation(CliErrors.CLUSTER_NAME_ERROR).AsException();
            }

            // enter registry uri for docker registry
            Console.Write("Api Server Uri: ");
            var apiServerUri = Console.ReadLine();

            // check registryUri not empty
            if (string.IsNullOrEmpty(apiServerUri))
            {
                throw ErrorDefinition.Validation(CliErrors.CLUSTER_URI_ERROR).AsException();
            }

            // get the registries in authorized context
            var cluster = await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Builder(profile);

                // create the cluster
                return client.CreateCluster(status.AccessToken, new CreateKubernetesCluster
                {
                    Name = name,
                    ApiServerUri = apiServerUri,
                    KubeConfigPlaintext = contents
                });
            });

            Console.WriteLine($"Kubernetes Cluster id: {cluster.Id}");

            return 0;
        }
    }
}