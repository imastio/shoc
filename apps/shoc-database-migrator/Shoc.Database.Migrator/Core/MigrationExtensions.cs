using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Shoc.ApiCore;
using Shoc.Data.Sql;
using Shoc.Database.Migrator.Config;

namespace Shoc.Database.Migrator.Core;

/// <summary>
/// The migration extensions
/// </summary>
public static class MigrationExtensions
{
    /// <summary>
    /// Adds migration pipeline to the services
    /// </summary>
    /// <param name="services">The services</param>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    public static IServiceCollection AddMigrations(this IServiceCollection services, IConfiguration configuration)
    {
        // get migration settings
        var migrationSettings = configuration.BindAs<MigrationSettings>("Migration");

        // get settings from configuration
        var dataSettings = configuration.BindAs<DataSourceSettings>("DataSource");

        // register settings for future use
        services.AddSingleton(dataSettings);
        services.AddSingleton(migrationSettings);

        // create new connection string builder
        var connectionStringBuilder = new MySqlConnectionStringBuilder(dataSettings.ConnectionString)
        {
            Database = dataSettings.Database
        };

        // add fluent migrator for mysql based on given connection string
        services.AddFluentMigratorCore()
            .ConfigureRunner(builder => builder.AddMySql5()
                .WithGlobalConnectionString(connectionStringBuilder.ToString())
                .ScanIn(typeof(MigrationManifest).Assembly).For.Migrations().For.VersionTableMetaData())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
        
        // return services for chaining
        return services;
    }
}
