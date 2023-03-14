using System.CommandLine;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The command to delete the user
    /// </summary>
    public class UserDeleteCommand : Command
    {
        /// <summary>
        /// Creates new instance of the command
        /// </summary>
        public UserDeleteCommand() : base("delete", "Delete existing user")
        {
            this.AddOption(new Option<string>(new[] { "-e", "--email" }, "The email of user to delete"));
        }
    }
}