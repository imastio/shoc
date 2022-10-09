using System;
using System.Threading;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shoc.Database.Migrator.Config;

namespace Shoc.Database.Migrator.Migrations.Core
{
    /// <summary>
    /// The migration hosted runner
    /// </summary>
    public class MigrationHostedRunner : IHostedService
    {
        /// <summary>
        /// The service provider instance
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// The host application lifetime
        /// </summary>
        private readonly IHostApplicationLifetime hostApplicationLifetime;

        /// <summary>
        /// Creates new instance of migration hosted runner
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <param name="hostApplicationLifetime">The host application lifetime</param>
        public MigrationHostedRunner(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
        {
            this.serviceProvider = serviceProvider;
            this.hostApplicationLifetime = hostApplicationLifetime;
        }

        /// <summary>
        /// Runs the migration hosted runner
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.hostApplicationLifetime.ApplicationStarted.Register(this.OnAppStarted);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handler to invoke on application start
        /// </summary>
        private void OnAppStarted()
        {
            // get migration settings
            var settings = this.serviceProvider.GetRequiredService<MigrationSettings>();

            // if migration on startup is not setup do not process
            if (!settings.MigrateOnStartup)
            {
                return;
            }

            // do within scope
            using (var scope = this.serviceProvider.CreateScope())
            {
                // get migration runner
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                // do migration
                runner.MigrateUp();
            }
            
            // if stop immediately is required 
            if (settings.StopOnMigrate)
            {
                // stop the application
                this.hostApplicationLifetime.StopApplication();
            }
        }

        /// <summary>
        /// Stops the migration hosted service
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            // do nothing
            return Task.CompletedTask;
        }
    }
}