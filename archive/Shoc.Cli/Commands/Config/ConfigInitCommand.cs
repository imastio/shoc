using System.CommandLine;

namespace Shoc.Cli.Commands.Config
{
    /// <summary>
    /// The configuration initialization command
    /// </summary>
    public class ConfigInitCommand : Command
    {
        /// <summary>
        /// The config initialization command
        /// </summary>
        public ConfigInitCommand() : base("init", "Initialize the configuration")
        {
        }
    }
}