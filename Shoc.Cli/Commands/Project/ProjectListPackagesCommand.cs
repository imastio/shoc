using System.CommandLine;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The command to list packages of the project
    /// </summary>
    public class ProjectListPackagesCommand : Command
    {
        /// <summary>
        /// Creates new instance of the command
        /// </summary>
        public ProjectListPackagesCommand() : base("list-packages", "List all the packages of the project")
        {
            this.AddOption(new Option<string>(new[] { "-n", "--name" }, "The name of project to list packages"));
        }
    }
}