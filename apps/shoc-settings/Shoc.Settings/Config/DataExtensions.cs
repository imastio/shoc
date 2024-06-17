using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.Access.Data;
using Shoc.Access.Data.Sql;
using Shoc.ApiCore;
using Shoc.Data.Sql;
using Shoc.DataProtection;
using Shoc.DataProtection.Sql;
using Shoc.Mailing;
using Shoc.Mailing.Sql;

namespace Shoc.Settings.Config
{
    /// <summary>
    /// The data configuration of system
    /// </summary>
    public static class DataConfiguration
    {
        /// <summary>
        /// Configure data environment
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="configuration">The configuration instance</param>
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            // get settings from configuration
            var dataSettings = configuration.BindAs<DataSourceSettings>("DataSource");

            // register settings for future use
            services.AddSingleton(dataSettings);

            // get the set of sources
            var sources = SettingsOperations.GetSources();

            // build and init new data operations instance
            var dataOps = OperationsInitializer.Init(dataSettings, sources.ToArray());

            // add data operations
            services.AddSingleton(dataOps);

            // add domain repositories
            services.AddSingleton<IProtectionKeyRepository, ProtectionKeyRepository>();
            services.AddSingleton<IAccessRepository, AccessRepository>();
            services.AddSingleton<IMailingProfileRepository, MailingProfileRepository>();
            
            // chain services
            return services;
        }
    }
}