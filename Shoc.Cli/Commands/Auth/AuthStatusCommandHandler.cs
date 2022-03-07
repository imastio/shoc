using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.Auth
{
    /// <summary>
    /// The handler for the user auth status
    /// </summary>
    public class AuthStatusCommandHandler : ShocCommandHandlerBase
    {
        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly AuthService authService;

        /// <summary>
        /// Creates new instance of user whoami command handler
        /// </summary>
        /// <param name="authService">The authentication service</param>
        public AuthStatusCommandHandler(AuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Implementation of user whoami command
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // get the current user
            var status = await this.authService.GetStatus(this.Profile);
            
            // write current claims
            context.Console.WriteLine($"Subject: {status.Id}");
            context.Console.WriteLine($"Name: {status.Name}");
            context.Console.WriteLine($"Email: {status.Email}");
            context.Console.WriteLine($"Username: {status.Username}");
            context.Console.WriteLine($"Expiration: {status.SessionExpiration.ToLocalTime()}");

            // all good
            return 0;
        }
    }
}