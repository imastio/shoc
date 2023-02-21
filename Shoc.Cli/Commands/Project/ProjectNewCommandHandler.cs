using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Builder.Model.Project;
using Shoc.Cli.Services;
using Shoc.Core;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The new project command handler
    /// </summary>
    public class ProjectNewCommandHandler : ProjectCommandHandlerBase
    {
        /// <summary>
        /// The given project name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The given project type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ProjectNewCommandHandler(ClientService clientService, AuthService authService) : base(clientService, authService)
        {
        }

        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // make sure name specified
            if (string.IsNullOrEmpty(this.Name))
            {
                throw ErrorDefinition.Validation(CliErrors.MISSING_PROJECT_NAME, "The project name is missing. Try adding --name option to specify one.").AsException();
            }

            // make sure type specified
            if (string.IsNullOrEmpty(this.Type))
            {
                throw ErrorDefinition.Validation(CliErrors.MISSING_PROJECT_TYPE, "The project type is missing. Try adding --type option to specify one.").AsException();
            }

            // do the operation authorized
            var result = await this.authService.DoAuthorized(this.Profile, async (profile, me) =>
            {
                // the project to create
                var project = new CreateProjectModel
                {
                    Name = this.Name,
                    Type = this.Type
                };
                return await this.clientService.Builder(profile).CreateProject(me.AccessToken, project);
            });
            
            Console.WriteLine($"The project {result.Name} was created.");
            return 0;
        }
    }
}