using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Shoc.Identity.Model;

namespace Shoc.ApiCore.Protection
{
    /// <summary>
    /// Role-based authorization 
    /// </summary>
    public class UserTypeAuthorization
    {
        /// <summary>
        /// The empty hash set
        /// </summary>
        private static readonly HashSet<string> EMPTY_SET = new();

        /// <summary>
        /// The priority mapping of user types
        /// </summary>
        private static readonly Dictionary<string, int> TYPE_ORDER = new()
        {
            { UserTypes.USER, 2 }, 
            { UserTypes.ADMIN, 1 },
            { UserTypes.ROOT, 0 }
        };

        /// <summary>
        /// Checks if claimed roles are satisfying role requirement
        /// </summary>
        /// <param name="context">The authorization context</param>
        /// <param name="minimumType">The given minimum type to check</param>
        /// <param name="allowInsiders">Allow insider access</param>
        /// <returns></returns>
        public static bool CheckType(AuthorizationFilterContext context, string minimumType, bool allowInsiders)
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

            // get scopes into a hash set
            var scopes = claims.GetValueOrDefault(KnownClaims.SCOPE, EMPTY_SET)
                .SelectMany(scope => scope.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                .ToHashSet();

            // if insider access is allowed and "svc" scope is given allow access
            if (allowInsiders && scopes.Contains(KnownScopes.SVC))
            {
                return true;
            }

            // if minimum type is not given consider requesting lowest as a minimum
            minimumType = string.IsNullOrWhiteSpace(minimumType) ? UserTypes.USER : minimumType;

            // get type if available
            var type = claims.GetValueOrDefault(KnownClaims.USER_TYPE, EMPTY_SET).FirstOrDefault();

            // if user type is not given in claims assume it has lowest type (external)
            type = string.IsNullOrWhiteSpace(type) ? UserTypes.USER : type;

            // get minimum required priority
            var minimumPriority = TYPE_ORDER.GetValueOrDefault(minimumType, int.MaxValue);

            // get the claimed type priority
            var typePriority = TYPE_ORDER.GetValueOrDefault(type, int.MaxValue);

            // allow only if claimed type has priority with lower value (lower priority value means high permission)
            return typePriority <= minimumPriority;
        }
    }
}