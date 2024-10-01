using System.CommandLine;

namespace Shoc.Cli.Commands.Registry
{
    /// <summary>
    /// The command to delete the registry
    /// </summary>
    public class RegistryDeleteCommand : Command
    {
        /// <summary>
        /// Creates new instance of the command
        /// </summary>
        public RegistryDeleteCommand() : base("delete", "Delete existing docker registry")
        {
            this.AddOption(new Option<string>(new[] { "-n", "--name" }, "The name of registry to delete"));
        }
    }
}