using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The handler for the user sign in
    /// </summary>
    public class UserSigninCommandHandler : ShocCommandHandlerBase
    {
        /// <summary>
        /// The authentication service
        /// </summary>
        private readonly AuthService authService;

        /// <summary>
        /// If sign-in should be silent
        /// </summary>
        public bool Silent { get; set; }

        /// <summary>
        /// Creates new instance of user sign-in command handler
        /// </summary>
        /// <param name="authService">The authentication service</param>
        public UserSigninCommandHandler(AuthService authService)
        {
            this.authService = authService;
        }

        /// <summary>
        /// Implementation of user sign-in command
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override async Task<int> InvokeAsync(InvocationContext context)
        {
            // in case if silent sign-in required
            if (this.Silent)
            {
                // do silently
                await this.authService.SignInSilent(this.Profile);
                return 0;
            }
            
            // start interactive sign-in otherwise
            await this.authService.SignIn(this.Profile);

            // all good
            return 0;
        }
    }
}