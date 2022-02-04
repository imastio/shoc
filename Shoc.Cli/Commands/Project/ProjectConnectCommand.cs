using System.CommandLine;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The command to connect to a project
    /// </summary>
    public class ProjectConnectCommand : Command
    {
        /// <summary>
        /// Creates new instance of project connect command
        /// </summary>
        public ProjectConnectCommand() : base("connect", "Connect the local directory into a given project")
        {
            this.AddOption(new Option<string>(new []{ "--id" }, "The id of project to connect to"));
        }
    }
}