using System.CommandLine;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The command to create the user
    /// </summary>
    public class UserCreateCommand : Command
    {
        /// <summary>
        /// Creates new instance command
        /// </summary>
        public UserCreateCommand() : base("create", "Create new user")
        {
        }
    }
}