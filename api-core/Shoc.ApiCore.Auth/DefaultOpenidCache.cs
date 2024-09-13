using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Shoc.ApiCore.Auth;

/// <summary>
/// The default implementation for OpenId cache
/// </summary>
public class DefaultOpenidCache : IOpenidCache
{
    /// <summary>
    /// The cache entry key prefix
    /// </summary>
    /// <returns></returns>
    private static readonly string TOKEN_KEY_PREFIX = "c-tok-";

    /// <summary>
    /// The cache entry key prefix
    /// </summary>
    /// <returns></returns>
    private static readonly string TOKEN_ENDPOINT_KEY_PREFIX = "a-t-e-";

    /// <summary>
    /// The memory-backed cache to store tokens
    /// </summary>
    private readonly IMemoryCache cache;

    /// <summary>
    /// Creates new instance of cache storage
    /// </summary>
    /// <param name="cache">The memory cache instance</param>
    public DefaultOpenidCache(IMemoryCache cache)
    {
        this.cache = cache;
    }

    /// <summary>
    /// Gets the cached token or fallback value
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="scopes">The scopes to authorize with</param>
    /// <param name="fallbackValue">The fallback value</param>
    /// <returns></returns>
    public string GetTokenOr(string client, string[] scopes, string fallbackValue = null)
    {
        return this.cache.TryGetValue(TokenKey(client, scopes), out var value) ? value as string : fallbackValue;
    }

    /// <summary>
    /// Cache the given token value with the supplier
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="scopes">The scopes to authorize with</param>
    /// <param name="supplier">The supplier</param>
    /// <param name="expirationUtc">The expiration time</param>
    /// <returns></returns>
    public string CacheToken(string client, string[] scopes, Func<string> supplier, DateTimeOffset expirationUtc)
    {
        return this.cache.Set(TokenKey(client, scopes), supplier?.Invoke(), expirationUtc);
    }

    /// <summary>
    /// Gets the cached token endpoint or fallback value
    /// </summary>
    /// <param name="authority">The authority identifier</param>
    /// <param name="fallbackValue">The fallback value</param>
    /// <returns></returns>
    public string GetTokenEndpointOr(string authority, string fallbackValue = null)
    {
        return this.cache.TryGetValue(TokenEndpointKey(authority), out var value) ? value as string : fallbackValue;
    }

    /// <summary>
    /// Cache the given token endpoint value with the supplier
    /// </summary>
    /// <param name="authority">The authority</param>
    /// <param name="supplier">The supplier</param>
    /// <param name="expirationUtc">The expiration time</param>
    /// <returns></returns>
    public string CacheTokenEndpoint(string authority, Func<string> supplier, DateTimeOffset expirationUtc)
    {
        return this.cache.Set(TokenEndpointKey(authority), supplier?.Invoke(), expirationUtc);
    }

    /// <summary>
    /// Builds the cache key for the token
    /// </summary>
    /// <param name="client">The client</param>
    /// <param name="scopes">The scopes to of the token</param>
    /// <returns></returns>
    private static string TokenKey(string client, string[] scopes)
    {
        return $"{TOKEN_KEY_PREFIX}{client}-s-{string.Join('-', scopes.Order())}";
    }

    /// <summary>
    /// Builds the cache key for token endpoint
    /// </summary>
    /// <param name="authority">The authority</param>
    /// <returns></returns>
    private static string TokenEndpointKey(string authority)
    {
        return $"{TOKEN_ENDPOINT_KEY_PREFIX}{authority}";
    }
}
