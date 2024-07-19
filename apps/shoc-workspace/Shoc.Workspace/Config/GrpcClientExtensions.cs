using Microsoft.Extensions.DependencyInjection;
using Shoc.ApiCore.Grpc;
using Shoc.Identity.Grpc.Users;

namespace Shoc.Workspace.Config;

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
        services.AddDiscoverableGrpcClient("shoc-identity", channel => new UserServiceGrpc.UserServiceGrpcClient(channel));
        return services;
    }
}