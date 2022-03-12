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
        /// Allows initializing if manifest is missing
        /// </summary>
        public bool Init { get; set; }

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
            // try get existing manifest
            var manifest = await this.GetManifest();

            // manifest does not exist but could be initialized
            if (manifest == null && this.Init)
            {
                manifest = await this.SaveManifest(this.InitialManifest());
            }

            // make sure manifest exists
            if (manifest == null)
            {
                throw ErrorDefinition.Validation(CliErrors.MISSING_MANIFEST, "The manifest is missing. Try adding --init option to create new one.").AsException();
            }

            // do the operation authorized
            var result = await this.authService.DoAuthorized(this.Profile, async (profile, me) =>
            {
                // the project to create
                var project = new CreateProjectModel
                {
                    Name = manifest.Name,
                    Directory = manifest.Directory,
                    OwnerId = me.Id
                };
                return await this.clientService.Builder(profile).CreateProject(me.AccessToken, project);
            });
            
            Console.WriteLine($"The project {result.Name} was created in directory {result.Directory}");
            return 0;
        }

        /// <summary>
        /// Generate an initial manifest file
        /// </summary>
        /// <returns></returns>
        private ShocManifest InitialManifest()
        {
            // get the project name
            var name = this.Name ?? this.Directory.Name;

            // remove unsafe characters and replace with underscore
            name = name.Replace(".", "_").Replace(" ", "_");

            return new ShocManifest
            {
                Name = name,
                Directory = "/",
                Build = new BuildSpec
                {
                    Base = string.Empty,
                    User = string.Empty,
                    Hooks = new BuildHooksSpec
                    {
                        BeforePackage = new List<string>()
                    },
                    Input = new BuildInputSpec
                    {
                        Copy = new List<FileCopySpec>()
                    }
                },
                Run = new RunSpec
                {
                    Output = new RunOutputSpec
                    {
                        StdOut = string.Empty,
                        StdErr = string.Empty,
                        RequiredFiles = new List<string>()
                    },
                    Requests = new RunResourcesSpec()
                }
            };
        }
    }
}