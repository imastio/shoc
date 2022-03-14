using System.CommandLine;

namespace Shoc.Cli.Commands.Project
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
            this.AddOption(new Option<string>(new[] { "--include-shared" }, "Include shared registries"));
        }
    }
}