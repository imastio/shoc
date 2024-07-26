using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shoc.Core.Discovery;

namespace Shoc.ApiCore.GrpcClient;

/// <summary>
/// Extensions for Grpc clients
/// </summary>
public static class GrpcClientExtensions
{
    /// <summary>
    /// Configure grpc clients
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="service">The service to register</param>
    /// <param name="creator">The creator of the client</param>
    public static IServiceCollection AddDiscoverableGrpcClient<TClient>(this IServiceCollection services, string service, Func<CallInvoker, TClient> creator) where TClient: ClientBase<TClient>
    {
        // add client exception interceptor
        services.TryAddSingleton<GrpcClientExceptionInterceptor>();
        
        // add grpc client provider
        services.TryAddSingleton<IGrpcClientProvider, GrpcClientProvider>();
        
        // register client as a singleton 
        return services.AddSingleton(serviceProvider =>
        {
            // get the discovery
            var discovery = serviceProvider.GetRequiredService<IDiscoveryClient>();
            
            // do a sync call to get next base url
            var baseUrl = discovery.GetGrpcUrl(service).Result;

            // create a channel for a base url with insecure channel because grpc will be invoked only inside internal cluster
            var channel = GrpcChannel.ForAddress(new Uri(baseUrl), new GrpcChannelOptions
            {
                UnsafeUseInsecureChannelCallCredentials = true
            });

            // get the client interceptor
            var exceptionInterceptor = serviceProvider.GetRequiredService<GrpcClientExceptionInterceptor>();

            // add interceptor and get the invoker instance
            var invoker = channel.Intercept(exceptionInterceptor);
                
            // create the client
            var client = creator(invoker);
            
            return client;
        });

    }
}