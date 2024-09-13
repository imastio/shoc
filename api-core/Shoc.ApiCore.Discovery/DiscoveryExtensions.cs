using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.Core.Discovery;
using Shoc.Core.Discovery.Impl;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Shoc.ApiCore.Discovery;

/// <summary>
/// The discovery configuration helpers
/// </summary>
public static class DiscoveryExtensions
{
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

        // get the type
        var type = settings.Type;

        // check primary if allowed
        if (!"static".Equals(type))
        {
            throw new Exception($"The {type} is not valid primary discovery. Only static is supported for now.");
        }
        
        // get static settings
        var staticSettings = configuration.BindAs<StaticDiscoveryOptions>("StaticDiscovery");

        // register static settings
        services.AddSingleton(staticSettings);

        // add service definitions to the DI
        services.AddSingleton(GetServiceDefinitions());

        // add service registry
        services.AddSingleton<IServiceRegistry, StaticServiceRegistry>();

        // add static discovery client
        services.AddSingleton<IDiscoveryClient, StaticDiscoveryClient>();

        return services;
    }
    
    /// <summary>
    /// Gets the data sources declared in a module
    /// </summary>
    /// <returns></returns>
    public static ServiceDefinitions GetServiceDefinitions()
    {
        // get the execution directory
        var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        // return the 
        var file = Path.Combine(sourceDirectory, "Definitions", "services.yml");
        
        // create a deserializer
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .Build();

        // deserialize definitions
        return deserializer.Deserialize<ServiceDefinitions>(File.ReadAllText(file));
    }
}
