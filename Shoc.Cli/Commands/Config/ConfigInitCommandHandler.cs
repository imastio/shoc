using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Shoc.Cli.Services;

namespace Shoc.Cli.Commands.User
{
    /// <summary>
    /// The handler for the config init
    /// </summary>
    public class ConfigInitCommandHandler : ICommandHandler
    {
        /// <summary>
        /// The configuration service
        /// </summary>
        private readonly ConfigurationService configurationService;

        /// <summary>
        /// Creates new instance of config init handler
        /// </summary>
        /// <param name="configurationService">The configuration service</param>
        public ConfigInitCommandHandler(ConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        /// <summary>
        /// Implementation of config init command
        /// </summary>
        /// <param name="context">The invocation context</param>
        /// <returns></returns>
        public async Task<int> InvokeAsync(InvocationContext context)
        {
            // get or create a command
            var config = await this.configurationService.GetOrCreateConfiguration();
            
            // notify
            context.Console.WriteLine($"Configuration initialized. Default Profile: {config.DefaultProfile}");

            // all good
            return 0;
        }
    }
}