using System.CommandLine.Invocation;
using System.Threading.Tasks;
using ConsoleTables;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The user list command handler
    /// </summary>
    public class UserListCommandHandler : ShocCommandHandlerBase
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
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public UserListCommandHandler(ClientService clientService, AuthService authService) 
        {
            this.clientService = clientService;
            this.authService = authService;
        }

        /// <summary>
        /// Implementation of command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // get the users in authorized context
            var users = await this.authService.DoAuthorized(this.Profile, (profile, status) => 
                this.clientService.Identity(profile).GetUsers(status.AccessToken));

            // add headers
            var table = new ConsoleTable("Id", "Full Name", "Email", "Username", "Type", "Last IP");

            // add users to table
            foreach (var user in users)
            {
                table.AddRow(user.Id, user.FullName, user.Email, user.Username, user.Type, user.LastIp);
            }

            // print table
            table.Write();

            return 0;
        }
    }
}