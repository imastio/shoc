using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Shoc.Core;
using Shoc.Core.OpenId;

namespace Shoc.ApiCore.Auth;

/// <summary>
/// The authorized connection module
/// </summary>
public class AuthProvider
{
    /// <summary>
    /// The token expiration threshold
    /// </summary>
    private static readonly TimeSpan TOKEN_EXPIRE_THRESHOLD = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The default metadata expiration
    /// </summary>
    private static readonly TimeSpan METADATA_EXPIRATION = TimeSpan.FromMinutes(10);

    /// <summary>
    /// The authentication settings
    /// </summary>
    private readonly AuthSettings auth;

    /// <summary>
    /// The client settings
    /// </summary>
    private readonly ClientSettings settings;

    /// <summary>
    /// The openid cache storage
    /// </summary>
    private readonly IOpenidCache openidCache;

    /// <summary>
    /// The http client instance
    /// </summary>
    private readonly HttpClient httpClient;

    /// <summary>
    /// Creates new instance of auth provider
    /// </summary>
    /// <param name="auth">The authentication settings</param>
    /// <param name="settings">The client settings</param>
    /// <param name="openidCache">The openid cache</param>
    public AuthProvider(AuthSettings auth, ClientSettings settings, IOpenidCache openidCache)
    {
        this.auth = auth;
        this.settings = settings;
        this.openidCache = openidCache;
        this.httpClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        });
    }

    /// <summary>
    /// Executes an operation with token supplied
    /// </summary>
    /// <typeparam name="T">The type of return value</typeparam>
    /// <param name="securedFunction">The secured operation to execute</param>
    /// <returns></returns>
    public async Task<T> DoAuthorized<T>(Func<string, Task<T>> securedFunction)
    {
        // try get cached token
        var token = this.openidCache.GetTokenOr(this.settings.ClientId) ?? await this.GetTokenAndCache();
        
        // make sure we have a token
        if (token == null)
        {
            throw ErrorDefinition.Access().AsException();
        }

        return await securedFunction(token);
    }
    
    /// <summary>
    /// Gets a token and cache
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetTokenAndCache()
    {
        try
        {
            // get the token endpoint either from cache or request new one
            var endpoint = this.openidCache.GetTokenEndpointOr(this.auth.Authority) ?? await this.GetTokenEndpointAndCache();

            // get token response
            var tokenResponse = await this.httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = endpoint,
                Scope = this.settings.Scope,
                ClientId = this.settings.ClientId,
                ClientSecret = this.settings.ClientSecret
            });

            // error while getting the token
            if (tokenResponse.IsError)
            {
                return null;
            }
            
            // get access token from response 
            var token = tokenResponse.AccessToken;

            // no token
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            // the token is valid until
            var validUntil = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            // consider reducing expiration for safety within some threshold
            var cacheExpireAt = validUntil.Subtract(TOKEN_EXPIRE_THRESHOLD);

            // try cache the value with expiration
            return this.openidCache.CacheToken(this.settings.ClientId, () => token, cacheExpireAt);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Gets the token endpoint and caches it
    /// </summary>
    /// <returns></returns>
    private async Task<string> GetTokenEndpointAndCache()
    {
        // try get the metadata
        var metadata = await this.httpClient.GetDiscoveryDocumentAsync(this.auth.Authority);

        // throw an exception if something goes wrong
        if (metadata.IsError)
        {
            throw ErrorDefinition.Access().AsException();
        }

        // the expiration of metadata
        var exp = DateTimeOffset.UtcNow.Add(METADATA_EXPIRATION);

        // return the cached token endpoint
        return this.openidCache.CacheTokenEndpoint(this.auth.Authority, () => metadata.TokenEndpoint, exp);
    }
}
