using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.Access.Data;
using Shoc.Access.Data.Sql;
using Shoc.ApiCore;
using Shoc.Data.Sql;
using Shoc.DataProtection;
using Shoc.DataProtection.Sql;
using Shoc.Job.Data;
using Shoc.Job.Data.Sql;
using Shoc.ObjectAccess.Package;
using Shoc.ObjectAccess.Sql.Package;
using Shoc.ObjectAccess.Sql.Workspace;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Job.Config;

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
        var sources = JobOperations.GetSources();
        
        // build and init new data operations instance
        var dataOps = OperationsInitializer.Init(dataSettings, sources.ToArray());

        // add data operations
        services.AddSingleton(dataOps);

        // add domain repositories
        services.AddSingleton<IProtectionKeyRepository, ProtectionKeyRepository>();
        services.AddSingleton<IAccessRepository, AccessRepository>();
        services.AddSingleton<IWorkspaceAccessRepository, WorkspaceAccessRepository>();
        services.AddSingleton<IPackageAccessRepository, PackageAccessRepository>();
        services.AddSingleton<ILabelRepository, LabelRepository>();
        services.AddSingleton<IGitRepoRepository, GitRepoRepository>();
        services.AddSingleton<IJobRepository, JobRepository>();
        services.AddSingleton<IJobTaskRepository, JobTaskRepository>();
        services.AddSingleton<IJobTaskStatusRepository, JobTaskStatusRepository>();
            
        // chain services
        return services;
    }
}