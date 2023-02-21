using System.CommandLine;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The command to create a project
    /// </summary>
    public class ProjectNewCommand : Command
    {
        /// <summary>
        /// Creates new instance of project new command
        /// </summary>
        public ProjectNewCommand() : base("new", "Create a new project in the given directory")
        {
            this.AddOption(new Option<string>(new []{ "-n", "--name" }, "The name of project to initialize"));
            this.AddOption(new Option<string>(new []{ "-t", "--type" }, "The type of project to initialize"));
        }
    }
}