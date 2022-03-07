using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The list projects command handler
    /// </summary>
    public class ProjectListCommandHandler : ProjectCommandHandlerBase
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
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ProjectListCommandHandler(ClientService clientService, AuthService authService)
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
            var existing = await this.GetManifest();

            // do the operation authorized
            var result = await this.authService.DoAuthorized(this.Profile, async (profile, me) => await this.clientService.Builder(profile).GetMyAllProjects(me.AccessToken));

            // print all the projects
            foreach (var project in result)
            {
                // message postfix for selected one
                var postfix = existing?.Name == project.Name && existing?.Directory == project.Directory ? " [Current]" : string.Empty;

                // print project
                context.Console.WriteLine($"{project.Id} \t\t {project.Directory} \t\t {project.Name}{postfix}");
            }
            
            return 0;
        }
    }
}