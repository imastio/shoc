using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Imast.Ext.DiscoveryCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore.Grpc;

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
    public static IServiceCollection AddDiscoverableGrpcClient<TClient>(this IServiceCollection services, string service, Func<GrpcChannel, TClient> creator) where TClient: ClientBase<TClient>
    {
        // register client as a singleton 
        return services.AddSingleton(serviceProvider =>
        {
            // get the discovery
            var discovery = serviceProvider.GetRequiredService<IDiscoveryClient>();

            // do a sync call to get next base url
            var baseUrl = discovery.GetNextBaseUrl(service).Result;

            // create a channel for a base url
            var channel = GrpcChannel.ForAddress(new Uri(baseUrl));

            // create the client
            var client =  creator(channel);

            
            return client;
        });
    }
}