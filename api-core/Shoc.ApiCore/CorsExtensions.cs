using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore;

/// <summary>
/// The CORS extensions
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Adds cors policy to the system
    /// </summary>
    /// <param name="services">The services</param>
    /// <param name="name">The name of policy</param>
    /// <returns></returns>
    public static IServiceCollection AddAnyOriginCors(this IServiceCollection services, string name)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy(name,
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }
}
