using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Shoc.Identity.Model;

namespace Shoc.ApiCore.Protection
{
    /// <summary>
    /// Role-based authorization 
    /// </summary>
    public class RoleAuthorization
    {
        /// <summary>
        /// The empty hash set
        /// </summary>
        private static readonly HashSet<string> EMPTY_SET = new();

        /// <summary>
        /// Checks if claimed roles are satisfying role requirement
        /// </summary>
        /// <param name="context">The authorization context</param>
        /// <param name="definedRoles">The given roles to check</param>
        /// <param name="requireAll">Indicates if all roles are required</param>
        /// <param name="allowInsiders">Allow insider access</param>
        /// <returns></returns>
        public static bool CheckRoles(AuthorizationFilterContext context, ISet<string> definedRoles, bool requireAll, bool allowInsiders)
        {
            // group by claims type
            var claims = context.HttpContext.User
                .Claims
                .GroupBy(claim => claim.Type)
                .ToDictionary(g => g.Key, g => g.Select(claim => claim.Value).ToHashSet());

            // get roles if available
            var roles = claims.GetValueOrDefault(KnownClaims.ROLE, EMPTY_SET);

            // get scopes into a hash set
            var scopes = claims.GetValueOrDefault(KnownClaims.SCOPE, EMPTY_SET)
                .SelectMany(scope => scope.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                .ToHashSet();

            // if insider access is allowed and "svc" scope is given allow access
            if (allowInsiders && scopes.Contains(KnownScopes.SVC))
            {
                return true;
            }

            // root is allowed for everything
            if (roles.Contains(Roles.ROOT))
            {
                return true;
            }
            
            // allow access if given a role that is in allowed role set
            if (!requireAll && definedRoles.Any(roles.Contains))
            {
                return true;
            }

            // allow access if all the given roles are in required set
            if (requireAll && definedRoles.All(roles.Contains))
            {
                return true;
            }

            return false;
        }
    }
}