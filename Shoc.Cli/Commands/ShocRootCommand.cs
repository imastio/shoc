using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.IO;
using Shoc.Cli.Commands.Auth;
using Shoc.Cli.Commands.Config;
using Shoc.Cli.Commands.Project;

namespace Shoc.Cli.Commands
{
    /// <summary>
    /// The shoc root command
    /// </summary>
    public class ShocRootCommand : RootCommand
    {
        /// <summary>
        /// Creates new instance of root command
        /// </summary>
        public ShocRootCommand() : base("The Shoc Command-line Tool")
        {
            // add command subtrees
            this.AddCommand(BuildProjectCommands());
            this.AddCommand(BuildAuthCommands());
            this.AddCommand(BuildConfigCommands());

            // add default handler
            this.Handler = CommandHandler.Create((Action) (() => this.Invoke("-h")));

            // add the profile as an option for all
            this.AddOption(new Option<string>(new[] { "-p", "--profile" }, "Select the profile"));
        }

        /// <summary>
        /// Creates project commands subtree
        /// </summary>
        /// <returns></returns>
        private static Command BuildProjectCommands()
        {
            // a wrapper command for project commands
            var command = new Command("project", "The project commands")
            {
                new ProjectNewCommand(),
                new ProjectListCommand(),
                new ProjectDeleteCommand()
            };

            // add directory as an option to all commands
            command.AddOption(new Option<DirectoryInfo>(new[] { "-d", "--directory" }, GetCurrentDirectory,  "The context directory"));
            
            // return the wrapper command 
            return command;
        }

        /// <summary>
        /// Creates auth commands subtree
        /// </summary>
        /// <returns></returns>
        private static Command BuildAuthCommands()
        {
            // a wrapper command for project commands
            var command = new Command("auth", "The user authentication commands")
            {
                new AuthSigninCommand(),
                new AuthSignoutCommand(),
                new AuthStatusCommand()
            };
            
            // return the wrapper command 
            return command;
        }

        /// <summary>
        /// Creates config commands subtree
        /// </summary>
        /// <returns></returns>
        private static Command BuildConfigCommands()
        {
            // a wrapper command for project commands
            var command = new Command("config", "The configuration commands")
            {
                new ConfigInitCommand()
            };

            // return the wrapper command 
            return command;
        }


        /// <summary>
        /// Gets the current directory by default
        /// </summary>
        /// <returns></returns>
        private static DirectoryInfo GetCurrentDirectory()
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory());
        }
    }
}