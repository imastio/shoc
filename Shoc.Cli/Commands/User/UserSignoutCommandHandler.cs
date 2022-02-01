using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The handler for the user sign out
    /// </summary>
    public class UserSignoutCommandHandler : ShocCommandHandlerBase
    {
        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly AuthService authService;

        /// <summary>
        /// Creates new instance of user sign-out command handler
        /// </summary>
        /// <param name="authService">The authentication service</param>
        public UserSignoutCommandHandler(AuthService authService)
        {
            this.authService = authService;
        }
        
        /// <summary>
        /// Implementation of sign-out command
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // start sign-in
            await this.authService.SignOut(this.Profile);

            // all good
            return 0;
        }
    }
}