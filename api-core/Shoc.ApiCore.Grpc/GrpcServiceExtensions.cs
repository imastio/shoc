using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore.Grpc;

/// <summary>
/// Extensions for Grpc services
/// </summary>
public static class GrpcServiceExtensions
{
    /// <summary>
    /// Adds the grpc essentials (basic services, interceptors, reflection) to the services
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <returns></returns>
    public static IServiceCollection AddGrpcEssentials(this IServiceCollection services)
    {
        // add grpc with interceptors
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GrpcServiceExceptionInterceptor>();
        });
        services.AddGrpcReflection();

        return services;
    }
}