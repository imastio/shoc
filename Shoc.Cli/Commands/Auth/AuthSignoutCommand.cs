using System.CommandLine;

namespace Shoc.Cli.Commands.Auth
{
    /// <summary>
    /// The command to sign-out the user
    /// </summary>
    public class AuthSignoutCommand : Command
    {
        /// <summary>
        /// Creates new instance of user sign-out command
        /// </summary>
        public AuthSignoutCommand() : base("sign-out", "Signs out the user")
        {
        }
    }
}