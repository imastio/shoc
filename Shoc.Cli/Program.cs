using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shoc.Cli.Commands;
using Shoc.Cli.Config;
using Shoc.Cli.Services;
using Shoc.Cli.System;

namespace Shoc.Cli
{
    /// <summary>
    /// The console application for Shoc interaction
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the application
        /// </summary>
        /// <param name="args">The CLI arguments</param>
        public static async Task Main(string[] args)
        {
            // configure command line builder
            var parser = BuildCli()
                .UseHost(_ => Host.CreateDefaultBuilder(args), builder => builder.UseCommands().ConfigureServices(ConfigureServices))
                .UseDefaults()
                .Build();
            
            // run the parser on command line arguments
            await parser.InvokeAsync(args);
        }

        /// <summary>
        /// Builds the command line tree
        /// </summary>
        /// <returns></returns>
        private static CommandLineBuilder BuildCli()
        {
            // create new builder
            return new CommandLineBuilder(new ShocRootCommand());
        }

        /// <summary>
        /// Configures the services and the host context
        /// </summary>
        /// <param name="context">The host builder context</param>
        /// <param name="services">The services collection</param>
        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddProtection();

            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<NetworkService>();
            services.AddSingleton<EncryptedStorage>();
            services.AddSingleton<AuthService>();
        }
    }
}
