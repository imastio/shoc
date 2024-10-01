using System.CommandLine;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The command to list the projects
    /// </summary>
    public class ProjectListCommand : Command
    {
        /// <summary>
        /// Creates new instance of project list command
        /// </summary>
        public ProjectListCommand() : base("list", "Lists all the projects of the user")
        {
        }
    }
}