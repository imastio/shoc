using System.CommandLine;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The command to package the project
    /// </summary>
    public class ProjectPackageCommand : Command
    {
        /// <summary>
        /// Creates new instance of project package command
        /// </summary>
        public ProjectPackageCommand() : base("package", "Create a package for the project")
        {
        }
    }
}