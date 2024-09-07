using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Shoc.Registry.Model.TokenSpec;

namespace Shoc.Registry.Utility;

/// <summary>
/// The authentication utility
/// </summary>
public static class AuthenticationUtility
{
    /// <summary>
    /// Gets the basic credentials from authorization header
    /// </summary>
    /// <param name="headers">The headers dictionary</param>
    /// <returns></returns>
    public static BasicCredentials GetBasicCredentials(this IHeaderDictionary headers)
    {
        // get the authorization header
        var auth = headers.Authorization.FirstOrDefault() ?? string.Empty;
        
        // check if authorization header is basic as we support only basic auth
        if (!auth.StartsWith("basic", StringComparison.InvariantCultureIgnoreCase))
        {
            return BasicCredentials.EMPTY;
        } 
        
        // split by empty spaces
        var authParts = auth.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // length is not valid
        if (authParts.Length != 2)
        {
            return BasicCredentials.EMPTY;
        }

        // credentials by default
        string credentials;

        try
        {
            // try get credentials from base64 encoded string
            credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authParts[1]));
        }
        catch (Exception)
        {
            return BasicCredentials.EMPTY;
        }

        // split the credentials into parts
        var parts = credentials.Split(':', StringSplitOptions.RemoveEmptyEntries);
        
        // assign username and password from parts
        return new BasicCredentials
        {
            Username = parts[0],
            Password = parts.Length > 1 ? parts[1] : string.Empty
        };
    }
    
    /// <summary>
    /// Parses the scope claim into set of scope objects
    /// </summary>
    /// <param name="scopeClaim">The scope claim</param>
    /// <returns></returns>
    public static IEnumerable<RegistryAuthScope> ParseScopes(string scopeClaim)
    {
        // the result list
        var scopes = new List<RegistryAuthScope>();

        // consider empty scopes for the empty claim
        if (string.IsNullOrWhiteSpace(scopeClaim))
        {
            return scopes;
        }

        // split multiple scope entries by space
        var scopeEntries = scopeClaim.Split(' ');

        // handle each separate scope from entries
        foreach (var scope in scopeEntries)
        {
            // break into parts with ':' symbol
            var parts = scope.Split(':');

            // we require exact 3 parts
            if (parts.Length != 3)
            {
                continue;
            }
            
            // split the last part by colon to get actions
            var actions = parts[2].Split(',').ToArray();

            // replace wildcard with pull and push actions
            if (actions.Contains("*"))
            {
                actions = ["pull", "push"];
            }

            // add the resulting scope
            scopes.Add(new RegistryAuthScope
            {
                Type = parts[0],
                Name = parts[1],
                Actions = actions
            });
        }

        return scopes;
    }
    
    /// <summary>
    /// Convert access claims to the combined scope
    /// </summary>
    /// <param name="accesses">The set of accesses</param>
    /// <returns></returns>
    public static string ToScope(IEnumerable<AccessTokenAccessClaimSpec> accesses)
    {
        // the scope parts joined into one
        return string.Join(' ', accesses.Select(access => $"{access.Type}:{access.Name}:{string.Join(',', access.Actions)}"));
    }
}