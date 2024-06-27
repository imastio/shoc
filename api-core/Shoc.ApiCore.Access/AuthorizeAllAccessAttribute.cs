using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shoc.Core;

namespace Shoc.ApiCore.Access;

/// <summary>
/// The attribute to authorize any given access
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizeAllAccessAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    /// <summary>
    /// The allowed scopes for access
    /// </summary>
    public string[] AllowedScopes { get; set; }

    /// <summary>
    /// The set of authorization accesses
    /// </summary>
    private readonly HashSet<string> accesses;

    /// <summary>
    /// Creates new instance of bearer authorization guard
    /// </summary>
    public AuthorizeAllAccessAttribute(params string[] accesses)
    {
        this.AllowedScopes = [];
        this.accesses = accesses.ToHashSet();
    }

    /// <summary>
    /// The authorization logic
    /// </summary>
    /// <param name="context">The context of authorization</param>
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // check if role authorization is satisfied pass the filter
        if (AccessAuthorization.CheckAccess(context, this.accesses, true, this.AllowedScopes?.ToHashSet() ?? new HashSet<string>()))
        {
            return Task.CompletedTask;
        }

        // forbid access if role requirement is not satisfied
        context.Result = new JsonResult(AggregateErrorDefinition.Of(ErrorKinds.ACCESS_DENIED))
        {
            StatusCode = 403
        };

        return Task.CompletedTask;
    }
}
