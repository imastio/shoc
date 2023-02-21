using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The delete project command handler
    /// </summary>
    public class ProjectDeleteCommandHandler : ProjectCommandHandlerBase
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
        public ProjectDeleteCommandHandler(ClientService clientService, AuthService authService) : base(clientService, authService)
        {
        }

        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // get project
            var project = await this.RequireProject(this.Name);

            // do authorized action
            var result = await this.authService.DoAuthorized(this.Profile, async (profile, status) =>
            {
                // get the builder client
                var builder = this.clientService.Builder(profile);

                // delete the instance and get result back
                return await builder.DeleteProjectById(status.AccessToken, project.Id);
            });
            
            Console.WriteLine($"The project {result.Name} was deleted");
            return 0;
        }
    }
}