using System.CommandLine.Hosting;
using Microsoft.Extensions.Hosting;
using Shoc.Cli.Commands.Auth;
using Shoc.Cli.Commands.Config;
using Shoc.Cli.Commands.Project;
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

            builder.UseCommandHandler(typeof(AuthSigninCommand), typeof(AuthSigninCommandHandler));
            builder.UseCommandHandler(typeof(AuthSignoutCommand), typeof(AuthSignoutCommandHandler));
            builder.UseCommandHandler(typeof(AuthStatusCommand), typeof(AuthStatusCommandHandler));

            builder.UseCommandHandler(typeof(ConfigInitCommand), typeof(ConfigInitCommandHandler));



            return builder;
        }
    }
}