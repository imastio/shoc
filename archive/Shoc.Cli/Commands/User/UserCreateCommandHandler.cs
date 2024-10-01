using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;
using Shoc.Cli.Utility;
using Shoc.Core;
using Shoc.Identity.Model;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The handler for the user create
    /// </summary>
    public class UserCreateCommandHandler : ShocCommandHandlerBase
    {
        /// <summary>
        /// The client service
        /// </summary>
        protected readonly ClientService clientService;

        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly AuthService authService;

        /// <summary>
        /// Creates new instance of user create command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The authentication service</param>
        public UserCreateCommandHandler(ClientService clientService, AuthService authService)
        {
            this.clientService = clientService;
            this.authService = authService;
        }

        /// <summary>
        /// Implementation of user create command
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // enter full name
            Console.Write("Full Name: ");
            var fullName = Console.ReadLine();

            // check full name not empty
            if (string.IsNullOrEmpty(fullName))
            {
                throw ErrorDefinition.Validation(CliErrors.USER_FULLNAME_ERROR).AsException();
            }

            // enter email
            Console.Write("Email: ");
            var email = Console.ReadLine();

            // check email not empty
            if (string.IsNullOrEmpty(email))
            {
                throw ErrorDefinition.Validation(CliErrors.USER_EMAIL_ERROR).AsException();
            }

            // enter username 
            Console.Write("Username: ");
            var username = Console.ReadLine();

            // enter password
            Console.Write("Password: ");
            var password = ShocConsole.GetPassword();

            // make sure password is given
            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorDefinition.Validation(CliErrors.USER_PASSWORD_ERROR).Throw();
            }

            // break console line after getting password
            Console.WriteLine();

            // create the user in authorized context
            var user = await this.authService.DoAuthorized(this.Profile, (profile, status) =>
            {
                // get the client to identity
                var client = this.clientService.Identity(profile);

                // create registry
                return client.CreateUser(status.AccessToken, new CreateUserModel
                {
                    FullName = fullName,
                    Email = email,
                    Username = username,
                    Password = password,
                    EmailVerified = true
                });
            });

            // print added user id
            Console.WriteLine($"User id: {user.Id}");

            // all good
            return 0;
        }
    }
}