using System.CommandLine.Hosting;
using Microsoft.Extensions.Hosting;
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
            builder.UseCommandHandler(typeof(ProjectConnectCommand), typeof(ProjectConnectCommandHandler));
            builder.UseCommandHandler(typeof(ProjectListCommand), typeof(ProjectListCommandHandler));

            builder.UseCommandHandler(typeof(UserSigninCommand), typeof(UserSigninCommandHandler));
            builder.UseCommandHandler(typeof(UserSignoutCommand), typeof(UserSignoutCommandHandler));
            builder.UseCommandHandler(typeof(UserWhoamiCommand), typeof(UserWhoamiCommandHandler));

            builder.UseCommandHandler(typeof(ConfigInitCommand), typeof(ConfigInitCommandHandler));



            return builder;
        }
    }
}