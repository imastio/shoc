using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shoc.ObjectAccess.Package;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.ApiCore.ObjectAccess;

/// <summary>
/// The object access configuration
/// </summary>
public static class ObjectAccessExtensions
{
    /// <summary>
    /// Configure object access
    /// </summary>
    /// <param name="services">The services collection</param>
    public static IServiceCollection AddObjectAccessEssentials(this IServiceCollection services)
    {
        services.TryAddSingleton<WorkspacePermissionCalculator>();
        services.TryAddSingleton<PackagePermissionCalculator>();
        services.TryAddSingleton<IWorkspaceAccessEvaluator, WorkspaceAccessEvaluator>();
        services.TryAddSingleton<IPackageAccessEvaluator, PackageAccessEvaluator>();

        return services;
    }
}
