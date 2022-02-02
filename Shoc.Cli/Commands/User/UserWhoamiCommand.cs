using System.CommandLine;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The command to show current user
    /// </summary>
    public class UserWhoamiCommand : Command
    {
        /// <summary>
        /// Creates new instance of who am i command
        /// </summary>
        public UserWhoamiCommand() : base("whoami", "Who am i?")
        {
        }
    }
}