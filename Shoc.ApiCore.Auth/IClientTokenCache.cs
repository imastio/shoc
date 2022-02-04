using System;

namespace Shoc.ApiCore.Auth
{
    /// <summary>
    /// The token cache interface
    /// </summary>
    public interface IClientTokenCache
    {
        /// <summary>
        /// Gets the cached client token
        /// </summary>
        /// <param name="client">The client name</param>
        /// <param name="fallbackValue">The fallback value if missing</param>
        /// <returns></returns>
        string GetOr(string client, string fallbackValue = null);

        /// <summary>
        /// Caches the token for the client with given supplier 
        /// </summary>
        /// <param name="client">The client</param>
        /// <param name="supplier">The supplier</param>
        /// <param name="expirationUtc">The expiration time</param>
        /// <returns></returns>
        string Cache(string client, Func<string> supplier, DateTimeOffset expirationUtc);
    }
}