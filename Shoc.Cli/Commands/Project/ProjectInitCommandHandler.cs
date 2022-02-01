using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Shoc.Cli.Commands.Project
{
    /// <summary>
    /// The common class for project command handlers
    /// </summary>
    public class ProjectInitCommandHandler  : ProjectCommandHandlerBase
    {
        /// <summary>
        /// The given project name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Implementation of project command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public override Task<int> InvokeAsync(InvocationContext context)
        {
            // get the project name
            var name = this.Name ?? this.Directory.Name;

            context.Console.WriteLine($"Invoked project init with name {name}");

            return Task.FromResult(0);
        }
    }
}