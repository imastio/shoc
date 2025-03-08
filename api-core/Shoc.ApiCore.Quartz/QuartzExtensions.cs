using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Simpl;
using Shoc.Data.Sql;
using Shoc.Quartz;

namespace Shoc.ApiCore.Quartz;

/// <summary>
/// The quartz registration extensions
/// </summary>
public static class QuartzExtensions
{
    /// <summary>
    /// Configure quartz essentials
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="configuration">The configuration instance</param>
    public static IServiceCollection AddQuartzEssentials(this IServiceCollection services, IConfiguration configuration)
    {
        // get settings from configuration
        var settings = configuration.BindAs<QuartzSettings>("Quartz");
        
        // register settings
        services.AddSingleton(settings);

        // register quartz scheduler instance
        services.AddQuartz(quartz =>
        {
            // the scheduler name (instance name)
            quartz.SchedulerName = settings.SchedulerName;

            // the instance id to be auto-generated
            quartz.SchedulerId = "AUTO";
            
            // use dedicated thread pool for tasks
            quartz.UseDedicatedThreadPool(pool =>
            {
                pool.MaxConcurrency = settings.MaxConcurrency;
            });

            // check configuration
            quartz.CheckConfiguration = true;
            
            // configure the store
            quartz.UsePersistentStore(s =>
            {
                // use mysql with default connection string
                s.UseMySql(mysql =>
                {
                    mysql.ConnectionString = SqlExt.BuildConnectionString(configuration.BindAs<DataSourceSettings>("DataSource"));
                });
                
                // validate schema
                s.PerformSchemaValidation = true;
                
                // serialize as string properties
                s.UseProperties = true;
                
                // retry interval
                s.RetryInterval = TimeSpan.FromSeconds(15);

                // clustering configuration
                s.UseClustering(c =>
                {
                    // check-in intervals
                    c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                    c.CheckinInterval = TimeSpan.FromSeconds(10);
                });
                
                // use System.Text.Json serializer
                s.UseSerializer<SystemTextJsonObjectSerializer>();
            });
        });

        // chain services
        return services;
    }

    /// <summary>
    /// Host quartz scheduler as a hosted service
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="configuration">The configuration instance</param>
    public static IServiceCollection AddQuartzHosting(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddQuartzHostedService(q =>
        {
            q.AwaitApplicationStarted = true;
        });
    }
}