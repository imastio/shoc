using System;
using System.Collections.Generic;
using Imast.Ext.DiscoveryCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.Core.Discovery;

namespace Shoc.ApiCore.Discovery;

/// <summary>
/// The discovery configuration helpers
/// </summary>
public static class DiscoveryExtensions
{
    /// <summary>
    /// The set of allowed primary discovery clients
    /// </summary>
    private static readonly ISet<string> ALLOWED_PRIMARY = new HashSet<string>{ "eureka", "static", "gateway" };

    /// <summary>
    /// The set of allowed fallback discovery clients
    /// </summary>
    private static readonly ISet<string> ALLOWED_FALLBACK = new HashSet<string> { "static", "gateway" };

    /// <summary>
    /// Configures discovery based on given settings 
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    public static IServiceCollection AddDiscovery(this IServiceCollection services, IConfiguration configuration)
    {
        // get discovery settings
        var settings = configuration.BindAs<DiscoverySettings>("Discovery");

        // primary client type
        var primary = settings.Primary?.ToLowerInvariant() ?? settings.Fallback?.ToLowerInvariant() ?? "static";

        // check primary if allowed
        if (!string.IsNullOrWhiteSpace(primary) && !ALLOWED_PRIMARY.Contains(primary))
        {
            throw new Exception($"The {primary} is not valid primary discovery");
        }
        
        // use static as fallback if not given
        var fallback = settings.Fallback?.ToLowerInvariant() ?? "static";

        // check fallback if allowed
        if (!string.IsNullOrWhiteSpace(fallback) &&!ALLOWED_FALLBACK.Contains(fallback))
        {
            throw new Exception($"The {primary} is not valid primary discovery");
        }

        // decide based on type
        return primary switch
        {
            "static" => services.AddSingleton(GetStaticDiscovery(configuration)),
            "gateway" => services.AddSingleton(GetGatewayDiscovery(configuration)),
            "eureka" => services.AddSingleton(sp =>
            {
                // require eureka client
                var eurekaClient = sp.GetRequiredService<Steeltoe.Discovery.Eureka.EurekaDiscoveryClient>();


                // get fallback client
                var fallbackClient = fallback == "gateway"
                    ? GetGatewayDiscovery(configuration)
                    : GetStaticDiscovery(configuration);

                // build and add new eureka discovery client
                return (IDiscoveryClient)new EurekaDiscoveryClient(eurekaClient, fallbackClient);
            }),
            _ => throw new Exception($"Something went wrong while creating {primary} discovery with fallback {fallback}")
        };
    }

    /// <summary>
    /// Gets the static discovery client
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    private static IDiscoveryClient GetStaticDiscovery(IConfiguration configuration)
    {
        // get settings
        var settings = configuration.BindAs<StaticDiscoverySettings>("StaticDiscovery");

        // build static discovery
        return new ShocStaticDiscovery(settings?.Protocol ?? "http", settings?.Host);
    }

    /// <summary>
    /// Gets the gateway discovery client
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    private static IDiscoveryClient GetGatewayDiscovery(IConfiguration configuration)
    {
        // get settings
        var gatewaySettings = configuration.BindAs<GatewayDiscoverySettings>("GatewayDiscovery");

        // gateway is not given
        if (string.IsNullOrWhiteSpace(gatewaySettings.Gateway))
        {
            throw new Exception("Could not instantiate gateway discovery: missing gateway.");
        }

        // build gateway discovery
        return new GatewayDiscoveryClient(gatewaySettings.Gateway);
    }
}
