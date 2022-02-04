using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore.Auth
{
    /// <summary>
    /// The authentication middleware extensions
    /// </summary>
    public static class AuthExtensions
    {
        /// <summary>
        /// Adds the authentication essentials to the system
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthenticationMiddleware(this IServiceCollection services, IConfiguration configuration)
        {
            // get settings
            var settings = configuration.BindAs<AuthSettings>("Auth");

            // add settings
            services.AddSingleton(settings);

            // default inbound claim mapping should be disabled
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            // add bearer authentication layer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // base-address of your identity provider
                    options.Authority = settings.Authority;

                    // if you are using API resources, you can specify the name here
                    options.Audience = settings.Audience;

                    // the IdentityServer4 emits a "typ" header by default, recommended extra check
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

                    // disabled audience validation
                    options.TokenValidationParameters.ValidateAudience = false;

                    // only if given allow self signed certificate
                    if (settings.AllowInsecure)
                    {
                        // allow raw http connections
                        options.RequireHttpsMetadata = false;

                        // hack to disable server certificate validation
                        options.BackchannelHttpHandler = new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
                        };

                        // do not validate issuer
                        options.TokenValidationParameters.ValidateIssuer = false;
                    }
                });

            return services;
        }
    }
}