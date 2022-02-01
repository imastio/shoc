using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The common class for project command handlers
    /// </summary>
    public abstract class ProjectCommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// The context directory
        /// </summary>
        public DirectoryInfo Directory { get; set; }

        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public abstract Task<int> InvokeAsync(InvocationContext context);
    }
}