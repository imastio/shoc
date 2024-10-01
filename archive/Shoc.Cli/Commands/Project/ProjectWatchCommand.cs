using System.CommandLine;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The command to watch for a project job output
    /// </summary>
    public class ProjectWatchCommand : Command
    {
        /// <summary>
        /// Creates new instance of project watch command
        /// </summary>
        public ProjectWatchCommand() : base("watch", "Watch the logs of project job")
        {
            this.AddOption(new Option<string>(new []{ "-j", "--job-id" }, "The id of job to watch."));
        }
    }
}