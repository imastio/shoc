using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore.RazorEngine;

/// <summary>
/// The razor engine extensions
/// </summary>
public static class RazorEngineExtensions
{
    /// <summary>
    /// Adds the razor engine to the pipeline
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <returns></returns>
    public static IServiceCollection AddRazorEngine(this IServiceCollection services)
    {
        return services.AddSingleton<IRazorEngine, RazorEngineMvc>();
    }
}