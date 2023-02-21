using System;

namespace Shoc.ApiCore.AuthClient
{
    /// <summary>
    /// The OpenId cache interface
    /// </summary>
    public interface IOpenidCache
    {
        /// <summary>
        /// Gets the cached token or fallback value
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="fallbackValue">The fallback value</param>
        /// <returns></returns>
        string GetTokenOr(string client, string fallbackValue = null);

        /// <summary>
        /// Cache the given token value with the supplier
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="supplier">The supplier</param>
        /// <param name="expirationUtc">The expiration time</param>
        /// <returns></returns>
        string CacheToken(string client, Func<string> supplier, DateTimeOffset expirationUtc);

        /// <summary>
        /// Gets the cached token endpoint or fallback value
        /// </summary>
        /// <param name="authority">The authority identifier</param>
        /// <param name="fallbackValue">The fallback value</param>
        /// <returns></returns>
        string GetTokenEndpointOr(string authority, string fallbackValue = null);

        /// <summary>
        /// Cache the given token endpoint value with the supplier
        /// </summary>
        /// <param name="authority">The authority</param>
        /// <param name="supplier">The supplier</param>
        /// <param name="expirationUtc">The expiration time</param>
        /// <returns></returns>
        string CacheTokenEndpoint(string authority, Func<string> supplier, DateTimeOffset expirationUtc);
    }
}
