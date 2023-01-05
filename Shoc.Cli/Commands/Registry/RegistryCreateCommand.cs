using System.CommandLine;

namespace Shoc.Cli.Commands.Registry
{
    /// <summary>
    /// The command to create the registry
    /// </summary>
    public class RegistryCreateCommand : Command
    {
        /// <summary>
        /// Creates new instance of the command
        /// </summary>
        public RegistryCreateCommand() : base("create", "Create new docker registry")
        {
        }
    }
}