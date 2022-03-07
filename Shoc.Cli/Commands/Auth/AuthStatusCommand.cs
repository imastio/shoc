using System.CommandLine;

namespace Shoc.Cli.Commands.Auth
{
    /// <summary>
    /// The command to show current user
    /// </summary>
    public class AuthStatusCommand : Command
    {
        /// <summary>
        /// Creates new instance of authentication status
        /// </summary>
        public AuthStatusCommand() : base("status", "The authentication status")
        {
        }
    }
}