using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Builder.Model.Project;
using Shoc.Cli.Model;
using Shoc.Cli.Services;
using Shoc.Core;
using Shoc.ModelCore;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The delete project command handler
    /// </summary>
    public class ProjectDeleteCommandHandler : ProjectCommandHandlerBase
    {
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
            // get the project instance
            var project = await this.WithProject((_, p) => Task.FromResult(p));

            // do authorized action
            var result = await this.authService.DoAuthorized(this.Profile, async (profile, status) =>
            {
                // get the builder client
                var builder = this.clientService.Builder(profile);

                // delete the instance and get result back
                return await builder.DeleteProjectById(status.AccessToken, project.Id);
            });
            
            Console.WriteLine($"The project {result.Name} was deleted from directory {result.Directory}");
            return 0;
        }
    }
}