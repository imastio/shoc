using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The handler for the user who am i
    /// </summary>
    public class UserWhoamiCommandHandler : ShocCommandHandlerBase
    {
        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly AuthService authService;

        /// <summary>
        /// Creates new instance of user whoami command handler
        /// </summary>
        /// <param name="authService">The authentication service</param>
        public UserWhoamiCommandHandler(AuthService authService)
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
            var whoami = await this.authService.GetWhoAmI(this.Profile);
            
            // write current claims
            context.Console.WriteLine($"Subject: {whoami.Id}");
            context.Console.WriteLine($"Name: {whoami.Name}");
            context.Console.WriteLine($"Email: {whoami.Email}");
            context.Console.WriteLine($"Username: {whoami.Username}");
            context.Console.WriteLine($"Expiration: {whoami.SessionExpiration.ToLocalTime()}");

            // all good
            return 0;
        }
    }
}