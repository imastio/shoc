using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Shoc.Core;

namespace Shoc.ApiCore.Access;

/// <summary>
/// The filtering attribute makes sure the bearer authentication 
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BearerOnlyAttribute : Attribute, IAuthorizeData, IAuthorizationFilter
{
    /// <summary>
    /// The policy required to match
    /// </summary>
    string IAuthorizeData.Policy
    {
        get => null;
        set { }
    }

    /// <summary>
    /// The role requirement from authorization context
    /// </summary>
    string IAuthorizeData.Roles
    {
        get => null;
        set { }
    }

    /// <summary>
    /// The authentication schemes
    /// </summary>
    public string AuthenticationSchemes { get; set; }
    
    /// <summary>
    /// Creates new bearer guard
    /// </summary>
    /// <param name="schemes">The bearer scheme names</param>
    public BearerOnlyAttribute(params string[] schemes)
    {
        this.AuthenticationSchemes = schemes.Length == 0 ? "Bearer" : string.Join(",", schemes);
    }

    /// <summary>
    /// Creates new bearer guard
    /// </summary>
    /// <param name="scheme">The bearer scheme name</param>
    public BearerOnlyAttribute(string scheme = null)
    {
        this.AuthenticationSchemes = string.IsNullOrWhiteSpace(scheme) ? "Bearer" : scheme;
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
