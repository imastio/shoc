using Duende.IdentityServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.ApiCore;

namespace Shoc.Identity.OpenId
{
    /// <summary>
    /// The Google provider
    /// </summary>
    public static class GoogleAuthExtended
    {
        /// <summary>
        /// Configure Google Auth Provider client
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="configuration">The configuration instance</param>
        public static IServiceCollection AddGoogleAuthEssentials(this IServiceCollection services,
            IConfiguration configuration)
        {
            // get settings from configuration
            var settings = configuration.BindAs<GoogleAuthSettings>("GoogleAuth");

            // register settings for future use
            services.AddSingleton(settings);

            if (!string.IsNullOrEmpty(settings.ClientId) && !string.IsNullOrEmpty(settings.ClientSecret))
            {
                services.AddAuthentication().AddGoogle("Google", options =>
                {
                    options.ClientId = settings.ClientId;
                    options.ClientSecret = settings.ClientSecret;
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                });
            }

            return services;
        }
    }
}