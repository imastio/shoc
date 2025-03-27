using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Shoc.Identity.Config.Oidc;

/// <summary>
/// Configuration for adding external Oidc authentication
/// </summary>
public static class ExternalOidcExtended
{
    /// <summary>
    /// Adds external OIDC essentials
    /// </summary>
    /// <param name="builder">The auth builder</param>
    /// <returns></returns>
    public static AuthenticationBuilder AddExternalOidcEssentials(this AuthenticationBuilder builder)
    {
        builder.Services.AddSingleton<DynamicOidcProviderAccessor>();
        builder.Services.AddSingleton<IOptionsMonitor<OpenIdConnectOptions>, DynamicOidcOptionsMonitor>();
        builder.Services.AddSingleton<IConfigureOptions<OpenIdConnectOptions>, DynamicOidcOptionsConfigurator>();
        builder.AddOpenIdConnect(OidcProviderConstants.DYNAMIC_OIDC_SCHEME, options =>
        {
            // if client is already configured keep as is
            if (!string.IsNullOrWhiteSpace(options.ClientId))
            {
                return;
            }
            
            // as a fallback register some placeholders
            options.ClientId = "placeholder";
            options.Configuration = new OpenIdConnectConfiguration();
        });
        
        return builder;
    }
}