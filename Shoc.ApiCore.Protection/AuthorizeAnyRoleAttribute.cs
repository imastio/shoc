using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shoc.ApiCore.Protection
{
    /// <summary>
    /// Authorization requires any of the required roles to be in claims
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeAnyRoleAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Disallow machine access
        /// </summary>
        public bool AllowInsiders { get; set; }

        /// <summary>
        /// The allowed roles to authorize
        /// </summary>
        private readonly HashSet<string> roles;

        /// <summary>
        /// Creates new instance of bearer authorization guard
        /// </summary>
        public AuthorizeAnyRoleAttribute(params string[] roles)
        {
            this.AllowInsiders = true;
            this.roles = roles.ToHashSet();
        }

        /// <summary>
        /// Checks the authorization logic
        /// </summary>
        /// <param name="context">The context</param>
        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            // check if role authorization is satisfied pass the filter
            if (RoleAuthorization.CheckRoles(context, this.roles, false, this.AllowInsiders))
            {
                return;
            }

            // forbid access if role requirement is not satisfied
            context.Result = new StatusCodeResult(403);
        }
    }
}