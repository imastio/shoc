using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;
using Shoc.Core;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The user delete command handler
    /// </summary>
    public class UserDeleteCommandHandler : ShocCommandHandlerBase
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
        /// The given email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        public UserDeleteCommandHandler(ClientService clientService, AuthService authService) 
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
            // make sure name is given
            if (string.IsNullOrEmpty(this.Email))
            {
                throw ErrorDefinition.Validation(CliErrors.USER_MISSING_EMAIL).AsException();
            }

            // get user by username in authorized context
            var user = (await this.authService.DoAuthorized(this.Profile, (profile, status) => 
                this.clientService.Identity(profile).GetUserByEmail(status.AccessToken, this.Email)));

            // make sure user exists
            if (user == null)
            {
                throw ErrorDefinition.NotFound(CliErrors.USER_EMAIL_ERROR).AsException();
            }

            // delete delete by id in authorized context
            user = (await this.authService.DoAuthorized(this.Profile, (profile, status) => 
                this.clientService.Identity(profile).DeleteUser(status.AccessToken, user.Id)));

            Console.WriteLine($"Deleted User with id: {user.Id}");

            return 0;
        }
    }
}