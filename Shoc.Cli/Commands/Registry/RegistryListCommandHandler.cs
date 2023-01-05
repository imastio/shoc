using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.Registry
{
    /// <summary>
    /// The registry list command handler
    /// </summary>
    public class RegistryListCommandHandler : ShocCommandHandlerBase
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
        public RegistryListCommandHandler(ClientService clientService, AuthService authService) 
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
            // get the registries in authorized context
            var registries = await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Builder(profile);

                // load registries of the project
                return client.GetRegistries(status.AccessToken);
            });

            // print header
            Console.WriteLine("Id\t\tName\t\tRegistry Uri\t\tRepository\t\tUser Name");

            // print packages
            foreach (var reg in registries)
            {
                Console.WriteLine($"{reg.Id}\t\t{reg.Name}\t\t{reg.RegistryUri}\t\t{reg.Repository}\t\t{reg.Username}");
            }

            return 0;
        }
    }
}