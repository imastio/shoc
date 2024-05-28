using Microsoft.Extensions.Configuration;

namespace Shoc.ApiCore;

/// <summary>
/// Extending configuration capabilities
/// </summary>
public static class ConfigurationExtended
{
    /// <summary>
    /// Bind the section of configuration to a concrete type 
    /// </summary>
    /// <param name="configuration">The configuration</param>
    /// <param name="name">The section name</param>
    /// <typeparam name="T">The type of settings</typeparam>
    /// <returns></returns>
    public static T BindAs<T>(this IConfiguration configuration, string name) where T : new()
    {
        // new instance for settings
        var config = new T();
        
        // bind configuration
        configuration.GetSection(name).Bind(config);

        // return connected configuration
        return config;
    }

    /// <summary>
    /// Just a shortcut to get rid of "unused param"
    /// </summary>
    /// <param name="configuration">The configuration instance</param>
    public static IConfiguration Use(this IConfiguration configuration)
    {
        return configuration;
    }
}
