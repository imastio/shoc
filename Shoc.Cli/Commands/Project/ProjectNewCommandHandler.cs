using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Builder.Model;
using Shoc.Cli.Model;
using Shoc.Cli.Services;
using Shoc.Cli.Utility;
using Shoc.Core;
using Shoc.ModelCore;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The new project command handler
    /// </summary>
    public class ProjectNewCommandHandler : ProjectCommandHandlerBase
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
        public string Name { get; set; }

        /// <summary>
        /// Allows overwriting in case of existing manifest
        /// </summary>
        public bool Overwrite { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ProjectNewCommandHandler(ClientService clientService, AuthService authService)
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
            // get the project name
            var name = this.Name ?? this.Directory.Name;

            // remove unsafe characters and replace with underscore
            name = name.Replace(".", "_")
                .Replace(" ", "_")
                .Replace("-", "_");

            // try get existing manifest
            var existing = await this.GetManifest();

            // if already exists and no overwrite is allowed raise an error
            if (existing != null && !this.Overwrite)
            {
                throw ErrorDefinition.Validation(CliErrors.EXISTING_MANIFEST, "The manifest already exists. Try apply --overwrite to save it anyways.").AsException();
            }

            // do the operation authorized
            var result = await this.authService.DoAuthorized(this.Profile, async (profile, me) =>
            {
                // use client service to create new project
                var project = await this.clientService.Builder(profile).CreateMyProject(me.AccessToken,
                    new CreateUpdateProjectModel
                    {
                        Name = name,
                        OwnerId = me.Id
                    });

                // create a manifest
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