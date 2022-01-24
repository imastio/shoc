using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shoc.ApiCore.Protection
{
    /// <summary>
    /// The subject-based authorization attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizedSubjectAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Creates new instance of bearer authorization guard
        /// </summary>
        public AuthorizedSubjectAttribute()
        {
            this.AuthenticationSchemes = "Bearer";
        }

        /// <summary>
        /// Checks the authorization logic
        /// </summary>
        /// <param name="context">The context</param>
        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
           // nothing to do
        }
    }
}