using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore.Access;

/// <summary>
/// The access protection middleware
/// </summary>
public static class AccessExtensions
{
    /// <summary>
    /// Adds access authorization services
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <returns></returns>
    public static IServiceCollection AddAccessAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAccessAuthorization, ContextAccessAuthorization>();

        return services;
    }
    
    /// <summary>
    /// An extension method to register access enrichment to the pipeline
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAccessEnrichment(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AccessEnrichmentMiddleware>();
    }
}