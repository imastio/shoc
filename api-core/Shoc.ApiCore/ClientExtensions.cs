using System;
using Microsoft.Extensions.DependencyInjection;
using Shoc.Core.Discovery;

namespace Shoc.ApiCore;

/// <summary>
/// The client extensions
/// </summary>
public static class ClientExtensions
{
    /// <summary>
    /// Adds single client of given type
    /// </summary>
    /// <param name="services">The services</param>
    /// <param name="supplier">The client supplier</param>
    /// <typeparam name="TClient">The client type</typeparam>
    /// <returns></returns>
    public static IServiceCollection AddClient<TClient>(this IServiceCollection services, Func<IDiscoveryClient, SelfSettings, TClient> supplier)
        where TClient : class
    {
        return services.AddSingleton(sp =>
        {
            // get the discovery
            var discovery = sp.GetRequiredService<IDiscoveryClient>();

            // get the self settings
            var selfSettings = sp.GetRequiredService<SelfSettings>();

            // create new client add register
            return supplier(discovery, selfSettings);
        });
    }
}
