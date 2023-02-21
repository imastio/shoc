using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Builder.Model.Project;
using Shoc.Cli.Services;
using Shoc.Core;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The common class for project command handlers
    /// </summary>
    public abstract class ProjectCommandHandlerBase : ICommandHandler
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
        /// The profile name
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        protected ProjectCommandHandlerBase(ClientService clientService, AuthService authService)
        {
            this.clientService = clientService;
            this.authService = authService;
        }

        /// <summary>
        /// Gets the project by name
        /// </summary>
        /// <param name="name">The project</param>
        /// <returns></returns>
        protected async Task<ProjectModel> RequireProject(string name)
        {
            // make sure name specified
            if (string.IsNullOrEmpty(name))
            {
                ErrorDefinition.Validation(CliErrors.MISSING_PROJECT_NAME).Throw();
            }

            // get project by name
            var project = (await this.authService.DoAuthorized(this.Profile, async (profile, status) =>
            {
                var builder = this.clientService.Builder(profile);

                return await builder.GetProjects(status.AccessToken, name);
            })).FirstOrDefault();

            // make sure project exists
            if (project == null)
            {
                ErrorDefinition.Validation(CliErrors.INVALID_PROJECT).Throw();
            }

            return project;
        }

        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public abstract Task<int> InvokeAsync(InvocationContext context);
    }
}