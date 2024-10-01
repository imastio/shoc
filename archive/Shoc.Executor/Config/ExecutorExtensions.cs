using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.ApiCore;

namespace Shoc.Executor.Config
{
    /// <summary>
    /// The executor extensions
    /// </summary>
    public static class ExecutorExtensions
    {
        /// <summary>
        /// Adds the executor essentials
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddExecutor(this IServiceCollection services, IConfiguration configuration)
        {
            // get executor settings
            var executor = configuration.BindAs<ExecutorSettings>("Executor");

            // add settings for future use
            services.AddSingleton(executor);

            // return services for chaining
            return services;
        }
    }
}