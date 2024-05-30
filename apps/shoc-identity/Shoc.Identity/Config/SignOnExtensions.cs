using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.ApiCore;
using Shoc.Core.Security;

namespace Shoc.Identity.Config;

/// <summary>
/// The security essentials configuration
/// </summary>
public static class SignOnExtensions
{
    /// <summary>
    /// Add sign-on essentials
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="configuration">The configuration</param>
    public static IServiceCollection AddSignOnEssentials(this IServiceCollection services, IConfiguration configuration)
    {
        // get sign-on settings
        var settings = configuration.BindAs<SignOnSettings>("SignOn");

        // add sign-on settings
        services.AddSingleton(settings);

        // register password hasher
        services.AddSingleton<IPasswordHasher>(new RfcPasswordHasher(100));

        // chain services
        return services;
    }
}