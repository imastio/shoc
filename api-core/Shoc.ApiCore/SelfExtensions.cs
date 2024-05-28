using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore;

/// <summary>
/// Extensions for self configuration
/// </summary>
public static class SelfExtensions
{
    /// <summary>
    /// Configures self attributes
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns></returns>
    public static IServiceCollection AddSelf(this IServiceCollection services, IConfiguration configuration)
    {
        // register self settings
        return services.AddSingleton(configuration.BindAs<SelfSettings>("Self"));
    }
}
