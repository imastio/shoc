using System.CommandLine;

namespace Shoc.Cli.Commands.Registry
{
    /// <summary>
    /// The command to list the registries
    /// </summary>
    public class RegistryListCommand : Command
    {
        /// <summary>
        /// Creates new instance of the command
        /// </summary>
        public RegistryListCommand() : base("list", "List all the available registries")
        {
        }
    }
}