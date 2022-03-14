using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The project list-packages command handler
    /// </summary>
    public class ProjectListPackagesCommandHandler : ProjectCommandHandlerBase
    {
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
            var project = await this.RequireProject();

            // get the packages in authorized context
            var packages = await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to builder
                var client = this.clientService.Builder(profile);

                // load packages of the project
                return client.GetPackages(status.AccessToken, project.Id);
            });

            // print header
            Console.WriteLine("Id\t\tStatus\t\tProgress\t\tProgress Message\t\tRegistry\t\tImage Uri");

            // print packages
            foreach (var package in packages)
            {
                Console.WriteLine($"{package.Id}\t\t{package.Status}\t\t{package.Progress}\t\t{package.ProgressMessage}\t\t{package.RegistryId}\t\t{package.ImageUri}");
            }

            return 0;
        }
    }
}