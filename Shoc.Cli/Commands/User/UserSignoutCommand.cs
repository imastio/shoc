using System.CommandLine;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The command to sign-out the user
    /// </summary>
    public class UserSignoutCommand : Command
    {
        /// <summary>
        /// Creates new instance of user sign-out command
        /// </summary>
        public UserSignoutCommand() : base("sign-out", "Signs out the user")
        {
        }
    }
}