using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Shoc.Cli.Commands
{
    /// <summary>
    /// The base command handler
    /// </summary>
    public abstract class ShocCommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// The selected profile
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Implementation of command invocation
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public abstract Task<int> InvokeAsync(InvocationContext context);
    }
}