using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The registry list command handler
    /// </summary>
    public class RegistryListCommandHandler : ProjectCommandHandlerBase
    {
        /// <summary>
        /// Include shared registries
        /// </summary>
        public bool IncludeShared { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public RegistryListCommandHandler(ClientService clientService, AuthService authService) : base(clientService, authService)
        {
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

                // load packages of the project
                return client.GetRegistries(status.AccessToken);
            });

            // print header
            Console.WriteLine("Id\t\tName\t\tRegistry Uri\t\tShared\t\tUser Name");

            // print packages
            foreach (var reg in registries)
            {
                Console.WriteLine($"{reg.Id}\t\t{reg.Name}\t\t{reg.RegistryUri}\t\t{reg.Shared}\t\t{reg.Username}");
            }

            return 0;
        }
    }
}