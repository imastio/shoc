using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.Identity.Client;

namespace Shoc.ApiCore.AuthClient
{
    public static class AuthClientExtensions
    {
        /// <summary>
        /// Adds the authentication client to the system
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationClient(this IServiceCollection services, IConfiguration configuration)
        {
            // bind client settings
            var settings = configuration.BindAs<ClientSettings>("Client");

            // keep settings 
            services.AddSingleton(settings);

            // make sure memory cache is in pipeline
            services.AddMemoryCache();

            // add token cache storage
            services.AddSingleton<IOpenidCache, DefaultOpenidCache>();

            // this will require Connect client to exist in the system
            services.AddSingleton<AuthProvider>();

            return services;
        }
    }
}
