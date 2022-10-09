using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shoc.ApiCore.Protection
{
    /// <summary>
    /// Authorization requires any of the required roles to be in claims
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeMinUserTypeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Disallow machine access
        /// </summary>
        public bool AllowInsiders { get; set; }

        /// <summary>
        /// The minimum type to authorize
        /// </summary>
        private readonly string type;

        /// <summary>
        /// Creates new instance of bearer authorization guard
        /// </summary>
        public AuthorizeMinUserTypeAttribute(string type)
        {
            this.AllowInsiders = true;
            this.type = type;
        }

        /// <summary>
        /// Checks the authorization logic
        /// </summary>
        /// <param name="context">The context</param>
        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            // check if role authorization is satisfied pass the filter
            if (UserTypeAuthorization.CheckType(context, this.type, this.AllowInsiders))
            {
                return;
            }

            // forbid access if role requirement is not satisfied
            context.Result = new StatusCodeResult(403);
        }
    }
}