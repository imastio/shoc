﻿using System;
using System.Collections.Generic;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shoc.Cli.Commands;
using Shoc.Cli.Config;
using Shoc.Cli.Services;
using Shoc.Cli.System;
using Shoc.Core;

namespace Shoc.Cli
{
    /// <summary>
    /// The console application for Shoc interaction
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The shoc command-line application
        /// </summary>
        /// <param name="args">The CLI arguments</param>
        public static async Task Main(string[] args)
        {
            // configure command line builder
            var parser = BuildCli()
                .UseDefaults()
                .UseExceptionHandler(HandleExceptions)
                .UseHost(_ => Host.CreateDefaultBuilder(args), builder => builder.UseCommands().ConfigureServices(ConfigureServices))
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
            // configure logging
            services.AddLogging(log =>
                log.AddFilter("Default", LogLevel.Information)
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning)
                .AddConsole());

            services.AddSingleton<ILogger>(sp => sp.GetRequiredService<ILogger<Program>>());

            services.AddProtection();

            services.AddSingleton<DiscoveryService>();
            services.AddSingleton<ClientService>();
            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<NetworkService>();
            services.AddSingleton<EncryptedStorage>();
            services.AddSingleton<AuthService>();
        }

        /// <summary>
        /// A global exception handler
        /// </summary>
        /// <param name="exception">The thrown exception</param>
        /// <param name="context">The invocation context</param>
        private static void HandleExceptions(Exception exception, InvocationContext context)
        {
            // the default errors for unhandled exception
            var errors = new List<ErrorDefinition> { ErrorDefinition.Unknown(CliErrors.UNKNOWN_ERROR, exception.Message)};

            // in case of shoc exception use given errors
            if (exception is ShocException shocException)
            {
                errors = shocException.Errors;
            }

            // set error color
            Console.ForegroundColor = ConsoleColor.Red;

            // print errors
            foreach (var error in errors)
            {
                context.Console.Error.WriteLine($"{error.Code}: {error.Message}");
            }

            // reset color to default
            Console.ResetColor();
        }
    }
}
