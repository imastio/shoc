using System;
using System.Collections.Generic;
using System.Linq;
using Duende.IdentityServer;
using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shoc.ApiCore;
using Shoc.Core.OpenId;
using Shoc.Identity.Provider.Services;
using Shoc.Identity.Provider.Stores;

namespace Shoc.Identity.Provider.Config;

/// <summary>
/// The identity configuration extensions
/// </summary>
public static class IdentityExtended
{
    /// <summary>
    /// Configure Identity Server
    /// </summary>
    /// <param name="services">The services collection</param>
    /// <param name="configuration">The configuration instance</param>
    public static IServiceCollection AddIdentityEssentials(this IServiceCollection services, IConfiguration configuration)
    {
        // get settings from configuration
        var settings = configuration.BindAs<IdentitySettings>("Identity");

        // register settings for future use
        services.AddSingleton(settings);

        // add in memory clients provider
        services.AddSingleton(new InMemoryClientsProvider(GetClients(settings)));

        // setup options
        var optionsSetup = new Action<IdentityServerOptions>(options =>
        {
            options.UserInteraction.LoginUrl = settings.SignInUrl;
            options.UserInteraction.LoginReturnUrlParameter = "return_url";
            options.UserInteraction.CustomRedirectReturnUrlParameter = "return_url";
            options.UserInteraction.LogoutUrl = settings.SignOutUrl;
            options.UserInteraction.LogoutIdParameter = "logout_id";
            options.UserInteraction.ErrorUrl = settings.ErrorUrl;
            options.UserInteraction.ErrorIdParameter = "error_id";
            options.Discovery.ShowApiScopes = false;
            options.Discovery.ShowGrantTypes = false;
            options.Discovery.ShowClaims = false;
            options.Discovery.ShowResponseModes = false;
            options.Discovery.ShowResponseTypes = false;
            options.Discovery.ShowTokenEndpointAuthenticationMethods = false;
            options.Discovery.ShowExtensionGrantTypes = false;
            options.Discovery.ShowIdentityScopes = false;
            options.KeyManagement.Enabled = true;
            options.KeyManagement.RotationInterval = TimeSpan.FromDays(90);
            options.KeyManagement.PropagationTime = TimeSpan.FromDays(14);
            options.KeyManagement.RetentionDuration = TimeSpan.FromDays(30);
            options.Authentication.CookieAuthenticationScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
        });

        // add CORS policy service
        services.AddSingleton<ICorsPolicyService>(sp => new DefaultCorsPolicyService(sp.GetRequiredService<ILogger<DefaultCorsPolicyService>>()));

        // add and configure identity server
        return services.AddIdentityServer(optionsSetup)
            .AddInMemoryApiResources(GetApiResources())
            .AddInMemoryApiScopes(GetApiScopes())
            .AddInMemoryIdentityResources(GetIdentityResources())
            .AddKeyManagement()
            .AddClientStore<ClientStore>()
            .AddPersistedGrantStore<PersistedGrantStore>()
            .AddRedirectUriValidator<RedirectUriValidator>()
            .AddProfileService<ProfileService>()
            .AddSigningKeyStore<SigningKeyStore>()
            .Services;
    }
    
    /// <summary>
    /// Adds local application protection
    /// </summary>
    /// <param name="services">The services collection</param>
    public static IServiceCollection AddLocalApiProtection(this IServiceCollection services)
    {
        // add local APIs protection's bearer scheme to 
        services.AddAuthentication().AddLocalApi(AuthenticationObjects.LocalApiScheme, _ => { });
        
        // return services for chaining
        return services;
    }
    
    /// <summary>
    /// Defining API resources for protection
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>();
    }

    /// <summary>
    /// Defining API scopes for grant
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
        {
            new(KnownScopes.SHOC, new []
            {
                KnownClaims.SUBJECT,
                KnownClaims.EMAIL,
                KnownClaims.EMAIL_VERIFIED,
                KnownClaims.USER_TYPE,
                KnownClaims.SERVICE_TYPE,
                KnownClaims.PREFERRED_USERNAME,
                KnownClaims.NAME
            }),
            new(KnownScopes.SVC, new []
            {
                KnownClaims.SERVICE_TYPE
            }),
            new(KnownScopes.INFRASTRUCTURE_READ),
            new(KnownScopes.INFRASTRUCTURE_WRITE)
        };
    }

    /// <summary>
    /// Defining set of identity resources
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<IdentityResource> GetIdentityResources()
    {
        var openid = new IdentityResources.OpenId();
        var email = new IdentityResources.Email();
        var profile = new IdentityResources.Profile();
        
        profile.UserClaims.Add(KnownClaims.USER_TYPE);
        
        return new List<IdentityResource> { openid, email, profile};
    }

    /// <summary>
    /// Gets built-in clients
    /// </summary>
    /// <param name="settings">The identity settings</param>
    /// <returns></returns>
    private static IEnumerable<Client> GetClients(IdentitySettings settings)
    {
        // set of built-in clients
        var builtin = new List<Client>();

        // try get m2m client
        var m2m = GetMachineToMachineClient(settings);

        // add m2m client if available
        if (m2m != null)
        {
            builtin.Add(m2m);
        }

        // try get interactive clients
        var interactive = GetInteractiveClient(settings);

        // add interactive client if available
        if (interactive != null)
        {
            builtin.Add(interactive);
        }
        
        return builtin;
    }

    /// <summary>
    /// Gets the m2m client if available
    /// </summary>
    /// <returns></returns>
    private static Client GetMachineToMachineClient(IdentitySettings settings)
    {
        // the client id
        var clientId = settings?.MachineToMachine?.ClientId;

        // the client secret
        var clientSecret = settings?.MachineToMachine?.ClientSecret;

        // client is not defined
        if (clientId == null || clientSecret == null)
        {
            return null;
        }

        // get expiration time in seconds
        var tokenExpiration = settings.MachineToMachine.AccessTokenExpiration ?? (int)TimeSpan.FromMinutes(5).TotalSeconds;

        // build the client
        return new Client
        {
            ClientId = clientId,
            ClientSecrets = new List<Secret> { new (clientSecret.Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AccessTokenLifetime = tokenExpiration,
            RequireClientSecret = true,
            RequirePkce = false,
            AllowedScopes = { KnownScopes.SVC, KnownScopes.SHOC }
        };
    }

    /// <summary>
    /// Gets the interactive client if available
    /// </summary>
    /// <returns></returns>
    private static Client GetInteractiveClient(IdentitySettings settings)
    {
        // the client id
        var clientId = settings?.InteractiveClient?.ClientId;

        // client is not defined
        if (clientId == null)
        {
            return null;
        }

        // allowed redirect paths
        var redirectPaths = settings.InteractiveClient?.RedirectPaths ?? string.Empty;
        
        // parse semicolon separated redirects
        var allowedRedirectPaths = redirectPaths.Split(";", StringSplitOptions.RemoveEmptyEntries);

        // all post-logout redirect URIs
        var logoutRedirectPaths = settings.InteractiveClient?.PostLogoutRedirectPaths ?? string.Empty;

        // parse URIs
        var allowedLogoutRedirectPaths = logoutRedirectPaths.Split(";", StringSplitOptions.RemoveEmptyEntries);

        // redirect hosts
        var redirectHosts = settings.InteractiveClient?.RedirectHosts ?? string.Empty;

        // parse allowed hosts
        var allowedRedirectHosts = redirectHosts.Split(";", StringSplitOptions.RemoveEmptyEntries);

        // all redirect uris
        var allowedRedirectUris = new List<string>();

        // all post-logout redirect uris
        var allowedLogoutRedirectUris = new List<string>();

        // for each host build full uris
        foreach(var host in allowedRedirectHosts)
        {
            // combine path with host
            allowedRedirectUris.AddRange(allowedRedirectPaths.Select(path => CombineUri(host, path)));
            allowedLogoutRedirectUris.AddRange(allowedLogoutRedirectPaths.Select(path => CombineUri(host, path)));
        }

        // get expiration time in seconds
        var tokenExpiration = settings.InteractiveClient?.AccessTokenExpiration ?? (int)TimeSpan.FromMinutes(5).TotalSeconds;
        
        // get expiration time of refresh token in seconds
        var refreshTokenExpiration = settings.InteractiveClient?.RefreshTokenExpiration ?? (int)TimeSpan.FromDays(30).TotalSeconds;

        // build the client
        return new Client
        {
            ClientId = clientId,
            ClientSecrets = new List<Secret> { new (clientId.Sha512()) },
            AllowedGrantTypes = GrantTypes.Code,
            RequireClientSecret = false,
            RequirePkce = true,  
            AllowPlainTextPkce = false,
            AllowedScopes = new List<string> { KnownScopes.OPENID, KnownScopes.EMAIl, KnownScopes.PROFILE, KnownScopes.SHOC },
            RedirectUris = allowedRedirectUris,
            AllowOfflineAccess = true,
            RefreshTokenUsage = TokenUsage.ReUse,
            RefreshTokenExpiration = TokenExpiration.Sliding,
            AlwaysIncludeUserClaimsInIdToken = true,
            AccessTokenLifetime = tokenExpiration,
            SlidingRefreshTokenLifetime = refreshTokenExpiration,
            PostLogoutRedirectUris = allowedLogoutRedirectUris
        };
    }

    /// <summary>
    /// Combine host and path together
    /// </summary>
    /// <param name="host">The host</param>
    /// <param name="path">The path</param>
    /// <returns></returns>
    private static string CombineUri(string host, string path)
    {
        // build a URI
        var uri = new UriBuilder(host)
        {
            Path = path
        };

        return uri.ToString();
    }
}
