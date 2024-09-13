using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shoc.Core;

namespace Shoc.ApiCore.Access;

/// <summary>
/// Authorization requires any of the required roles to be in claims
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizeMinUserTypeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    /// <summary>
    /// The allowed scopes for access
    /// </summary>
    public string[] AllowedScopes { get; set; }

    /// <summary>
    /// The minimum type to authorize
    /// </summary>
    private readonly string type;

    /// <summary>
    /// Creates new instance of bearer authorization guard
    /// </summary>
    public AuthorizeMinUserTypeAttribute(string type)
    {
        this.AllowedScopes = [];
        this.type = type;
    }

    /// <summary>
    /// Checks the authorization logic
    /// </summary>
    /// <param name="context">The context</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip if allowing anonymous access
        if (AccessAuthorization.AnonymousAllowed(context))
        {
            return;
        }
        
        // check if role authorization is satisfied pass the filter
        if (UserTypeAuthorization.CheckType(context.HttpContext, this.type, this.AllowedScopes?.ToHashSet() ?? new HashSet<string>()))
        {
            return;
        }
        
        // forbid access if role requirement is not satisfied
        context.Result = new JsonResult(AggregateErrorDefinition.Of(ErrorKinds.ACCESS_DENIED))
        {
            StatusCode = 403
        };
    }
}
