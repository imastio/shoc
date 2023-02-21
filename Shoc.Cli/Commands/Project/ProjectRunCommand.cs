using System.Collections.Generic;
using System.CommandLine;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The command to run a project package
    /// </summary>
    public class ProjectRunCommand : Command
    {
        /// <summary>
        /// Creates new instance of project run command
        /// </summary>
        public ProjectRunCommand() : base("run", "Run the project")
        {
            this.AddOption(new Option<string>(new []{ "-n", "--name" }, "The name of project to run."));
            this.AddOption(new Option<string>(new []{ "-v", "--version" }, "The name of package to run. Defaults to latest"));
            this.AddOption(new Option<IEnumerable<string>>(new []{ "-a", "--args" }, "The arguments to pass to the entrypoint executable.") { AllowMultipleArgumentsPerToken = true });
        }
    }
}