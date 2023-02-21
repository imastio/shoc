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
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public ProjectListCommandHandler(ClientService clientService, AuthService authService) : base(clientService, authService)
        {
        }

        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // do the operation authorized
            var result = await this.authService.DoAuthorized(this.Profile, async (profile, me) => await this.clientService.Builder(profile).GetProjects(me.AccessToken));

            // print header
            context.Console.WriteLine("Id\t\tName\t\tType");

            // print all the projects
            foreach (var project in result)
            {
                // print project
                context.Console.WriteLine($"{project.Id} \t\t {project.Type}");
            }
            
            return 0;
        }
    }
}