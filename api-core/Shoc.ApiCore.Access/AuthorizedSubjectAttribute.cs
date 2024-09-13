using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shoc.Core;

namespace Shoc.ApiCore.Access;

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
        var authenticated = context.HttpContext.User.Identity?.IsAuthenticated ?? false;

        if (!authenticated)
        {
            // forbid access if role requirement is not satisfied
            context.Result = new JsonResult(AggregateErrorDefinition.Of(ErrorKinds.ACCESS_DENIED))
            {
                StatusCode = 403
            };
        }
    }
}
