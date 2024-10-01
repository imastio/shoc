using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;
using Shoc.Cli.Utility;
using Shoc.Core;
using Shoc.Identity.Model;

namespace Shoc.Cli.Commands.Setup
{
    /// <summary>
    /// The create root command handler
    /// </summary>
    public class CreateRootCommandHandler : ShocCommandHandlerBase
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
        /// The configuration service
        /// </summary>
        private readonly ConfigurationService configurationService;

        /// <summary>
        /// Creates new instance of command handler
        /// </summary>
        /// <param name="clientService">The client service</param>
        /// <param name="authService">The auth service</param>
        /// <param name="configurationService">The configuration service</param>
        public CreateRootCommandHandler(ClientService clientService, AuthService authService, ConfigurationService configurationService) 
        {
            this.clientService = clientService;
            this.authService = authService;
            this.configurationService = configurationService;
        }

        /// <summary>
        /// Implementation of command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // enter full name for root user
            Console.Write("Full Name: ");
            var fullName = Console.ReadLine();

            // check name not empty
            if (string.IsNullOrEmpty(fullName))
            {
                throw ErrorDefinition.Validation(CliErrors.SETUP_MISSING_NAME).AsException();
            }

            // enter email for root user
            Console.Write("Email: ");
            var email = Console.ReadLine();

            // check name not empty
            if (string.IsNullOrEmpty(email))
            {
                throw ErrorDefinition.Validation(CliErrors.SETUP_MISSING_EMAIL).AsException();
            }

            // enter password for root user
            Console.Write("Password: ");
            var password = ShocConsole.GetPassword();

            // make sure password is given
            if (string.IsNullOrWhiteSpace(password))
            {
                ErrorDefinition.Validation(CliErrors.USER_PASSWORD_ERROR).Throw();
            }

            // break console line after getting password
            Console.WriteLine();

            // get the profile
            var profile = await this.configurationService.GetProfile(this.Profile);

            // create the root user in authorized context
            // get the client to identity
            var client = this.clientService.Identity(profile);

            // create root user
            var rootUser = await client.CreateRoot(new CreateRootModel
            {
                FullName = fullName,
                Email = email,
                Password = password
            });

            Console.WriteLine($"Root user id: {rootUser.Id}");

            return 0;
        }
    }
}