using Microsoft.Extensions.DependencyInjection;
using Shoc.ApiCore.GrpcClient;
using Shoc.Cluster.Grpc.Clusters;
using Shoc.Identity.Grpc.Users;
using Shoc.Package.Grpc.Packages;
using Shoc.Registry.Grpc.Registries;
using Shoc.Secret.Grpc.Secrets;
using Shoc.Workspace.Grpc.Workspaces;

namespace Shoc.Job.Config;

/// <summary>
/// The grpc clients extensions
/// </summary>
public static class GrpcClientExtensions
{
    /// <summary>
    /// Configure grpc clients
    /// </summary>
    /// <param name="services">The services collection</param>
    public static IServiceCollection AddGrpcClients(this IServiceCollection services)
    {
        services.AddDiscoverableGrpcClient("shoc-identity", invoker => new UserServiceGrpc.UserServiceGrpcClient(invoker));
        services.AddDiscoverableGrpcClient("shoc-workspace", invoker => new WorkspaceServiceGrpc.WorkspaceServiceGrpcClient(invoker));
        services.AddDiscoverableGrpcClient("shoc-workspace", invoker => new WorkspaceMemberServiceGrpc.WorkspaceMemberServiceGrpcClient(invoker));
        services.AddDiscoverableGrpcClient("shoc-registry", invoker => new WorkspaceDefaultRegistryServiceGrpc.WorkspaceDefaultRegistryServiceGrpcClient(invoker));
        services.AddDiscoverableGrpcClient("shoc-registry", invoker => new RegistryServiceGrpc.RegistryServiceGrpcClient(invoker));
        services.AddDiscoverableGrpcClient("shoc-registry", invoker => new RegistryPlainCredentialServiceGrpc.RegistryPlainCredentialServiceGrpcClient(invoker));
        services.AddDiscoverableGrpcClient("shoc-package", invoker => new PackageServiceGrpc.PackageServiceGrpcClient(invoker));
        services.AddDiscoverableGrpcClient("shoc-cluster", invoker => new ClusterServiceGrpc.ClusterServiceGrpcClient(invoker));
        services.AddDiscoverableGrpcClient("shoc-secret", invoker => new UnifiedSecretServiceGrpc.UnifiedSecretServiceGrpcClient(invoker));
        
        return services;
    }
}