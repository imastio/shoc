using System.CommandLine;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The command to delete a project
    /// </summary>
    public class ProjectDeleteCommand : Command
    {
        /// <summary>
        /// Creates new instance of project delete command
        /// </summary>
        public ProjectDeleteCommand() : base("delete", "Deletes the current project from the remote server")
        {
        }
    }
}