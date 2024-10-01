using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Builder.Model.Registry;
using Shoc.Cli.Services;
using Shoc.Cli.Utility;
using Shoc.Core;

namespace Shoc.Cli.Commands.Registry
{
    /// <summary>
    /// The registry create command handler
    /// </summary>
    public class RegistryCreateCommandHandler : ShocCommandHandlerBase
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
        public RegistryCreateCommandHandler(ClientService clientService, AuthService authService) 
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
            // enter name for docker registry
            Console.Write("Name: ");
            var name = Console.ReadLine();

            // check name not empty
            if (string.IsNullOrEmpty(name))
            {
                throw ErrorDefinition.Validation(CliErrors.REGISTRY_NAME_ERROR).AsException();
            }

            // enter registry uri for docker registry
            Console.Write("Registry Uri: ");
            var registryUri = Console.ReadLine();

            // check registryUri not empty
            if (string.IsNullOrEmpty(registryUri))
            {
                throw ErrorDefinition.Validation(CliErrors.REGISTRY_REGISTRY_URI_ERROR).AsException();
            }

            // enter name for docker registry
            Console.Write("Repository: ");
            var repository = Console.ReadLine();

            // check repository not empty
            if (string.IsNullOrEmpty(repository))
            {
                throw ErrorDefinition.Validation(CliErrors.REGISTRY_REPOSITORY_ERROR).AsException();
            }

            // enter name for docker registry
            Console.Write("Allow Nesting [y/N] ");
            var allowNesting = ShocConsole.Confirm();

            // break console line after getting confirmation
            Console.WriteLine();

            // enter email for docker registry
            Console.Write("Email: ");
            var email = Console.ReadLine();

            // enter username for docker registry
            Console.Write("Username: ");
            var username = Console.ReadLine();

            // enter username for docker registry
            Console.Write("Password: ");
            var password = ShocConsole.GetPassword();

            // break console line after getting password
            Console.WriteLine();

            // create the registry in authorized context
            var registry = await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Builder(profile);

                // create registry
                return client.CreateRegistry(status.AccessToken, new CreateDockerRegistry
                {
                    Name = name,
                    RegistryUri = registryUri,
                    Repository = repository,
                    AllowNesting = allowNesting,
                    Email = email,
                    Username = username,
                    PasswordPlaintext = password
                });
            });

            Console.WriteLine($"Docker Registry id: {registry.Id}");

            return 0;
        }
    }
}