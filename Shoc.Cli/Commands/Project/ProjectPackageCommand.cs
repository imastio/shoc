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
            this.AddOption(new Option<string>(new[] { "-n", "--name" }, "The name of package"));
            this.AddOption(new Option<string>(new[] { "-v", "--project-version" }, "The version of project"));
            this.AddOption(new Option<string>(new[] { "-d", "--target-directory" }, "The directory path to package"));
        }
    }
}