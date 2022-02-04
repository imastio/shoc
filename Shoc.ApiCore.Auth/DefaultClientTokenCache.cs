using System;
using Microsoft.Extensions.Caching.Memory;

namespace Shoc.ApiCore.Auth
{
    /// <summary>
    /// The default client token cache
    /// </summary>
    public class DefaultClientTokenCache : IClientTokenCache
    {
        /// <summary>
        /// The cache entry key prefix
        /// </summary>
        /// <returns></returns>
        private static readonly string KEY_PREFIX = "c-tok-";
            
        /// <summary>
        /// The memory-backed cache to store tokens
        /// </summary>
        private readonly IMemoryCache cache;

        /// <summary>
        /// Creates new instance of cache storage
        /// </summary>
        /// <param name="cache">The memory cache instance</param>
        public DefaultClientTokenCache(IMemoryCache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// Gets the cached token or fallback value
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="fallbackValue">The fallback value</param>
        /// <returns></returns>
        public string GetOr(string client, string fallbackValue = null)
        {
            return this.cache.TryGetValue(Key(client), out var value) ? value as string : fallbackValue;
        }

        /// <summary>
        /// Cache the given value with the supplier
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="supplier">The supplier</param>
        /// <param name="expirationUtc">The expiration time</param>
        /// <returns></returns>
        public string Cache(string client, Func<string> supplier, DateTimeOffset expirationUtc)
        {
            return this.cache.Set(Key(client), supplier?.Invoke(), expirationUtc);
        }

        /// <summary>
        /// Builds the cache key
        /// </summary>
        /// <param name="client">The client</param>
        /// <returns></returns>
        private static string Key(string client)
        {
            return $"{KEY_PREFIX}{client}";
        }
    }
}