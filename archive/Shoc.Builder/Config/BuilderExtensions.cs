using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.ApiCore;

namespace Shoc.Builder.Config
{
    /// <summary>
    /// The builder extensions
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// Adds the builder essentials
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddBuilder(this IServiceCollection services, IConfiguration configuration)
        {
            // get builder settings
            var builder = configuration.BindAs<BuilderSettings>("Builder");

            // add settings for future use
            services.AddSingleton(builder);

            // return services for chaining
            return services;
        }
    }
}