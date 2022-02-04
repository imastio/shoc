using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Model;
using Shoc.Cli.Services;
using Shoc.Cli.Utility;
using Shoc.ModelCore;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The project connect command handler
    /// </summary>
    public class ProjectConnectCommandHandler : ProjectCommandHandlerBase
    {
        /// <summary>
        /// The client service
        /// </summary>
        private readonly ClientService clientService;

        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly AuthService authService;

        /// <summary>
        /// The given project name
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ProjectConnectCommandHandler(ClientService clientService, AuthService authService)
        {
            this.clientService = clientService;
            this.authService = authService;
        }

        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // do the operation authorized
            var result = await this.authService.DoAuthorized(this.Profile, async (profile, me) =>
            {
                // use client service to create new project
                var project = await this.clientService.Builder(profile).GetMyProjectById(me.AccessToken, this.Id);

                // build a manifest
                return new ShocManifest
                {
                    Id = project.Id,
                    Build = Json.Deserialize<BuildSpec>(project.BuildSpec),
                    Run = Json.Deserialize<RunSpec>(project.RunSpec)
                };
            });

            // save the result
            await this.SaveManifest(result);

            return 0;
        }
    }
}