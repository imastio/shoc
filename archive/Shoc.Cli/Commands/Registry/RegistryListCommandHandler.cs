using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleTables;
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

            // add headers
            var table = new ConsoleTable("Id", "Name", "Registry Uri", "Repository", "User Name");

            // add registries
            foreach (var reg in registries)
            {
                table.AddRow(reg.Id, reg.Name, reg.RegistryUri, reg.Repository, reg.Username);
            }

            // print table
            table.Write();

            return 0;
        }
    }
}