using System.CommandLine;

namespace Shoc.Cli.Commands.Setup
{
    /// <summary>
    /// The command to create the root user
    /// </summary>
    public class CreateRootCommand : Command
    {
        /// <summary>
        /// Creates new instance command
        /// </summary>
        public CreateRootCommand() : base("create-root", "Create new root user")
        {
        }
    }
}