using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Shoc.Identity.OpenId
{
    /// <summary>
    /// The validator module to check if given redirect URI is valid
    /// </summary>
    public class RedirectUriValidator : IRedirectUriValidator
    {
        /// <summary>
        /// Checks if the given redirect URI is valid 
        /// </summary>
        /// <param name="requestedUri">The requested URI</param>
        /// <param name="client">The client to validate</param>
        /// <returns></returns>
        public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            // the set of allowed uris
            var allowedUris = client.RedirectUris?.ToList() ?? new List<string>();

            // try match strictly or based on CORS
            return MatchStrictOrLocal(requestedUri, allowedUris);
        }
        
        /// <summary>
        /// Checks if the given post-logout redirect URI is valid
        /// </summary>
        /// <param name="requestedUri">The redirect URI</param>
        /// <param name="client">The client</param>
        /// <returns></returns>
        public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            // the set of allowed uris
            var allowedUris = client.PostLogoutRedirectUris?.ToList() ?? new List<string>();

            // try match strictly or based on CORS
            return MatchStrictOrLocal(requestedUri, allowedUris);
        }

        /// <summary>
        /// Try match the given uri based on allowed redirect uris
        /// </summary>
        /// <param name="requestedUri">The requested URI</param>
        /// <param name="allowedRedirects">The allowed redirects</param>
        /// <returns></returns>
        protected static Task<bool> MatchStrictOrLocal(string requestedUri, ICollection<string> allowedRedirects)
        {
            // the requested uri
            var uri = new Uri(requestedUri);

            // build URIs from allowed redirect strings
            var allowed = allowedRedirects.Select(u => new Uri(u)).ToList();

            // if strict match then allow
            var strictMatch = ContainsExactUri(allowedRedirects, requestedUri);

            // on strict match allow
            if (strictMatch)
            {
                return Task.FromResult(true);
            }

            // checks if localhost is allowed in redirect uris
            var hasLocalhost = allowed.Any(u => u.IsLoopback);

            // in case if localhost is an allowed redirect and requested URI is localhost, allow if path match
            if (hasLocalhost && uri.IsLoopback)
            {
                return Task.FromResult(allowed.Any(u => uri.AbsolutePath.Equals(u.AbsolutePath, StringComparison.OrdinalIgnoreCase)));
            }
            
            // return result
            return Task.FromResult(false);
        }

        /// <summary>
        /// Checks if a given URI string is in a collection of strings (using ordinal ignore case comparison)
        /// </summary>
        /// <param name="uris">The uris.</param>
        /// <param name="requestedUri">The requested URI.</param>
        /// <returns></returns>
        protected static bool ContainsExactUri(ICollection<string> uris, string requestedUri)
        {
            return uris != null && uris.Contains(requestedUri, StringComparer.OrdinalIgnoreCase);
        }
    }
}