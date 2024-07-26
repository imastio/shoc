using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Shoc.Access.Model;
using Shoc.Core.OpenId;

namespace Shoc.ApiCore.Access;

/// <summary>
/// The scope based authorization
/// </summary>
public static class ScopeAuthorization
{
    /// <summary>
    /// The empty hash set
    /// </summary>
    private static readonly HashSet<string> EMPTY_SET = new();
    
    /// <summary>
    /// Checks if claimed scopes are satisfying scope requirement
    /// </summary>
    /// <param name="context">The authorization context</param>
    /// <param name="definedScopes">The given scopes to check</param>
    /// <param name="requireAll">Indicates if all scopes are required</param>
    /// <returns></returns>
    public static bool CheckScopes(HttpContext context, ISet<string> definedScopes, bool requireAll)
    {
        // group by claims type
        var claims = context.User
            .Claims
            .GroupBy(claim => claim.Type)
            .ToDictionary(g => g.Key, g => g.Select(claim => claim.Value).ToHashSet());

        // get types if available
        var types = claims.GetValueOrDefault(KnownClaims.USER_TYPE, EMPTY_SET);

        // get scopes into a hash set
        var scopes = claims.GetValueOrDefault(KnownClaims.SCOPE, EMPTY_SET)
            .SelectMany(scope => scope.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .ToHashSet();
        
        // root and admin is allowed for everything
        if (types.Contains(KnownUserTypes.ROOT) || types.Contains(KnownUserTypes.ADMIN))
        {
            return true;
        }
        
        // allow access if there is any scope that is in allowed set
        if (!requireAll && definedScopes.Any(scopes.Contains))
        {
            return true;
        }

        // allow access if all the scopes are in allowed set
        if (requireAll && definedScopes.All(scopes.Contains))
        {
            return true;
        }

        return false;
    }
}
