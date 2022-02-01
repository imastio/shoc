using System.CommandLine;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The command to sign-in the user
    /// </summary>
    public class UserSigninCommand : Command
    {
        /// <summary>
        /// Creates new instance of user sign-in command
        /// </summary>
        public UserSigninCommand() : base("sign-in", "Signs in the user")
        {
        }
    }
}