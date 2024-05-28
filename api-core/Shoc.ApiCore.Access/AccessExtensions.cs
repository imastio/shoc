using Microsoft.AspNetCore.Builder;

namespace Shoc.ApiCore.Access;

/// <summary>
/// The access protection middleware
/// </summary>
public static class AccessExtensions
{
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