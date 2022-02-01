using System.CommandLine;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The command to initialize a project
    /// </summary>
    public class ProjectInitCommand : Command
    {
        /// <summary>
        /// Creates new instance of project init command
        /// </summary>
        public ProjectInitCommand() : base("init", "Initialize a project in the given directory")
        {
            this.AddOption(new Option<string>(new []{ "-n", "--name" }, "The name of project to initialize"));
        }
    }
}