using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleTables;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The project list-packages command handler
    /// </summary>
    public class ProjectListPackagesCommandHandler : ProjectCommandHandlerBase
    {
        /// <summary>
        /// The package name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ProjectListPackagesCommandHandler(ClientService clientService, AuthService authService) : base(clientService, authService)
        {
        }

        /// <summary>
        /// Implementation of command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // requires the project in the context
            var project = await this.RequireProject(this.Name);

            // get the packages in authorized context
            var packages = await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Builder(profile);

                // load packages of the project
                return client.GetPackages(status.AccessToken, project.Id);
            });

            // add headers
            var table = new ConsoleTable("Id", "Status", "Progress", "Progress Message", "Registry", "Image Uri");

            // add packages
            foreach (var package in packages)
            {
                table.AddRow(package.Id, package.Status, package.Progress, package.ProgressMessage, package.RegistryId, package.ImageUri);
            }

            // print table
            table.Write();

            return 0;
        }
    }
}