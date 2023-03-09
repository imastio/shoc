using System.CommandLine.Hosting;
using Microsoft.Extensions.Hosting;
using Shoc.Cli.Commands.Auth;
using Shoc.Cli.Commands.Cluster;
using Shoc.Cli.Commands.Config;
using Shoc.Cli.Commands.Project;
using Shoc.Cli.Commands.Registry;
using Shoc.Cli.Commands.User;

namespace Shoc.Cli.Commands
{
    /// <summary>
    /// The command extensions
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Adds the command handlers into the chain of command line
        /// </summary>
        /// <param name="builder">The command line builder</param>
        /// <returns></returns>
        public static IHostBuilder UseCommands(this IHostBuilder builder)
        {
            builder.UseCommandHandler(typeof(ProjectNewCommand), typeof(ProjectNewCommandHandler));
            builder.UseCommandHandler(typeof(ProjectListCommand), typeof(ProjectListCommandHandler));
            builder.UseCommandHandler(typeof(ProjectDeleteCommand), typeof(ProjectDeleteCommandHandler));
            builder.UseCommandHandler(typeof(ProjectPackageCommand), typeof(ProjectPackageCommandHandler));
            builder.UseCommandHandler(typeof(ProjectListPackagesCommand), typeof(ProjectListPackagesCommandHandler));
            builder.UseCommandHandler(typeof(ProjectRunCommand), typeof(ProjectRunCommandHandler));
            builder.UseCommandHandler(typeof(ProjectWatchCommand), typeof(ProjectWatchCommandHandler));

            builder.UseCommandHandler(typeof(AuthSigninCommand), typeof(AuthSigninCommandHandler));
            builder.UseCommandHandler(typeof(AuthSignoutCommand), typeof(AuthSignoutCommandHandler));
            builder.UseCommandHandler(typeof(AuthStatusCommand), typeof(AuthStatusCommandHandler));

            builder.UseCommandHandler(typeof(RegistryListCommand), typeof(RegistryListCommandHandler));
            builder.UseCommandHandler(typeof(RegistryCreateCommand), typeof(RegistryCreateCommandHandler));
            builder.UseCommandHandler(typeof(RegistryDeleteCommand), typeof(RegistryDeleteCommandHandler));

            builder.UseCommandHandler(typeof(ClusterListCommand), typeof(ClusterListCommandHandler));
            builder.UseCommandHandler(typeof(ClusterCreateCommand), typeof(ClusterCreateCommandHandler));
            builder.UseCommandHandler(typeof(ClusterDeleteCommand), typeof(ClusterDeleteCommandHandler));


            builder.UseCommandHandler(typeof(ConfigInitCommand), typeof(ConfigInitCommandHandler));
            
            return builder;
        }
    }
}