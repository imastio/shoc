using System.CommandLine;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The command to list the users
    /// </summary>
    public class UserListCommand : Command
    {
        /// <summary>
        /// Creates new instance of the command
        /// </summary>
        public UserListCommand() : base("list", "List all the users")
        {
        }
    }
}