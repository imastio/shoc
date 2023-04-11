using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore
{
    /// <summary>
    /// Misc extensions for services
    /// </summary>
    public static class MiscExtensions
    {
        /// <summary>
        /// Restoring maximum number of forwarded headers.
        /// </summary>
        private const int MAX_FORWARDING_LIMIT = 5;

        /// <summary>
        /// Adds forwarding configuration to handle hosts under load balancing
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <returns></returns>
        public static IServiceCollection AddForwardingConfiguration(this IServiceCollection services)
        {
            return services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                options.ForwardLimit = MAX_FORWARDING_LIMIT;
            });
        }
    }
}