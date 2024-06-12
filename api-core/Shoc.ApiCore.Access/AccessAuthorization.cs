using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Shoc.Access.Model;
using Shoc.Core.OpenId;

namespace Shoc.ApiCore.Access;

/// <summary>
/// The access based authorization
/// </summary>
public static class AccessAuthorization
{
    /// <summary>
    /// The empty hash set
    /// </summary>
    private static readonly HashSet<string> EMPTY_SET = new();

    /// <summary>
    /// Checks if claimed access modifiers are satisfying access requirement
    /// </summary>
    /// <param name="context">The authorization context</param>
    /// <param name="definedAccesses">The given accesses to check</param>
    /// <param name="requireAll">Indicates if all accesses are required</param>
    /// <param name="allowedScopes">Allow service scopes</param>
    /// <returns></returns>
    public static bool CheckAccess(AuthorizationFilterContext context, ISet<string> definedAccesses, bool requireAll, ISet<string> allowedScopes)
    {
        // make sure allow anonymous is not defined to proceed the authorization
        if (context.Filters.Any(item => item is IAllowAnonymousFilter))
        {
            return true;
        }

        // make sure allow anonymous attribute is considered
        if (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            return true;
        }

        // group by claims type
        var claims = context.HttpContext.User
            .Claims
            .GroupBy(claim => claim.Type)
            .ToDictionary(g => g.Key, g => g.Select(claim => claim.Value).ToHashSet());

        // get types if available
        var types = claims.GetValueOrDefault(KnownClaims.USER_TYPE, EMPTY_SET);

        // get scopes into a hash set
        var scopes = claims.GetValueOrDefault(KnownClaims.SCOPE, EMPTY_SET)
            .SelectMany(scope => scope.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .ToHashSet();

        // if principal has scopes that is in allowed list, bypass the access
        if (allowedScopes.Overlaps(scopes))
        {
            return true;
        }
        
        // root and admin is allowed for everything
        if (types.Contains(KnownUserTypes.ROOT) || types.Contains(KnownUserTypes.ADMIN))
        {
            return true;
        }
        
        // get all granted accesses
        var grantedAccesses = context.HttpContext.GetItemOrDefault("Accesses", EMPTY_SET);
        
        // allow access if given an access that is in allowed access set
        if (!requireAll && definedAccesses.Any(grantedAccesses.Contains))
        {
            return true;
        }

        // allow access if all the given accesses are in required set
        if (requireAll && definedAccesses.All(grantedAccesses.Contains))
        {
            return true;
        }

        return false;
    }
}
