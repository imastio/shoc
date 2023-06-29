using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleTables;
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

            // add headers
            var table = new ConsoleTable("Id", "Name", "Type");

            // add projects
            foreach (var project in result)
            {
                table.AddRow(project.Id, project.Name, project.Type);
            }

            // print table
            table.Write();

            return 0;
        }
    }
}