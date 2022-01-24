using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shoc.ApiCore;
using Shoc.Identity.Model;

namespace Shoc.Identity.OpenId
{
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
            
            // setup options
            var optionsSetup = new Action<IdentityServerOptions>(options =>
            {
                options.IssuerUri = settings.Issuer ?? settings.PublicOrigin;
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
            });
            
            // add and configure identity server
            services.AddIdentityServer(optionsSetup)
                .AddInMemoryApiResources(GetApiResources())
                .AddInMemoryApiScopes(GetApiScopes())
                .AddInMemoryIdentityResources(GetIdentityResources())
                .AddSigningCredential(GetSigningKey(settings), IdentityServerConstants.ECDsaSigningAlgorithm.ES256)
                .AddInMemoryClients(GetClients(settings))
                .AddPersistedGrantStore<PersistedGrantStore>()
                .AddRedirectUriValidator<RedirectUriValidator>();
            //.AddProfileService<ProfileService>();

            services.AddSingleton<PublicOriginMiddleware>();
            services.AddSingleton<ICorsPolicyService, CorsPolicyService>();

            return services;
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
        /// Gets a signing key based on settings
        /// </summary>
        /// <param name="settings">The identity settings</param>
        /// <returns></returns>
        private static ECDsaSecurityKey GetSigningKey(IdentitySettings settings)
        {
            // source directory
            var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

            // build full path to key
            var fullKeyPath = Path.GetFullPath(settings.SigningKeyPath, sourceDirectory);

            // read signing key path to load key
            var eccPem = File.ReadAllText(fullKeyPath);

            // build key and import
            var edc = ECDsa.Create();

            // import pem content into key
            edc?.ImportFromPem(eccPem);

            // create a security key based on ECD 
            return new ECDsaSecurityKey(edc) { KeyId = "default" };
        }
        
        /// <summary>
        /// Defining API resources for protection
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ("shoc-identity", "The Shoc Identity API"),
                new ("shoc-builder", "The Shoc Builder API")
            };
        }

        /// <summary>
        /// Defining API scopes for grant
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new (KnownScopes.SVC, new []
                {
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified,
                    JwtClaimTypes.PreferredUserName,
                    JwtClaimTypes.Role
                })
            };
        }

        /// <summary>
        /// Defining set of identity resources
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };
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
            var clientId = settings?.MachineClient?.ClientId;

            // the client secret
            var clientSecret = settings?.MachineClient?.ClientSecret;

            // client is not defined
            if (clientId == null || clientSecret == null)
            {
                return null;
            }

            // get expiration time in seconds
            var tokenExpiration = settings.MachineClient.AccessTokenExpiration ?? (int)TimeSpan.FromMinutes(5).TotalSeconds;

            // build the client
            return new Client
            {
                ClientId = clientId,
                ClientSecrets = new List<Secret> { new (clientSecret.Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AccessTokenLifetime = tokenExpiration,
                RequireClientSecret = true,
                RequirePkce = false,
                AllowedScopes = { KnownScopes.SVC }
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

            // build the client
            return new Client
            {
                ClientId = clientId,
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,  
                AllowPlainTextPkce = false,
                AllowedScopes = new List<string> { "openid", "email", "profile" },
                RedirectUris = allowedRedirectUris,
                AllowOfflineAccess = false,
                AlwaysIncludeUserClaimsInIdToken = true,
                AccessTokenLifetime = tokenExpiration,
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
}