using System;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Cli.Services;
using Shoc.Core;

namespace Shoc.Cli.Commands.Registry
{
    /// <summary>
    /// The registry create command handler
    /// </summary>
    public class RegistryDeleteCommandHandler : ShocCommandHandlerBase
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
        /// The given registry name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public RegistryDeleteCommandHandler(ClientService clientService, AuthService authService) 
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
                throw ErrorDefinition.Validation(CliErrors.REGISTRY_NAME_ERROR).AsException();
            }

            // get registry by name in authorized context
            var registry = (await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Builder(profile);

                // load registry by name
                return client.GetRegistries(status.AccessToken, Name);
            })).FirstOrDefault();

            // delete registry by id in authorized context
            registry = (await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Builder(profile);

                // delete registry by id
                return client.DeleteRegistry(status.AccessToken, registry?.Id);
            }));

            Console.WriteLine($"Deleted Docker Registry id: {registry.Id}");

            return 0;
        }
    }
}