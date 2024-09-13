using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shoc.Core;

namespace Shoc.ApiCore.Access;

/// <summary>
/// The access authorization service based on accesses in context
/// </summary>t
public class ContextAccessAuthorization : IAccessAuthorization
{
    /// <summary>
    /// Require the principal to have any of the defined accesses
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    public async Task RequireAccessAny(HttpContext context, IEnumerable<string> requirements, IEnumerable<string> allowedScopes)
    {
        // if access check passes just return
        if (await this.CheckAccessAny(context, requirements, allowedScopes))
        {
            return;
        }

        throw ErrorDefinition.Access().AsException();
    }

    /// <summary>
    /// Require the principal to have any of the defined accesses
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    public async Task RequireAccessAll(HttpContext context, IEnumerable<string> requirements, IEnumerable<string> allowedScopes)
    {
        // if access check passes just return
        if (await this.CheckAccessAll(context, requirements, allowedScopes))
        {
            return;
        }

        throw ErrorDefinition.Access().AsException();
    }

    /// <summary>
    /// Require the principal to have any of the defined scopes
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <returns></returns>
    public async Task RequireScopesAny(HttpContext context, IEnumerable<string> requirements)
    {
        // if access check passes just return
        if (await this.CheckScopesAny(context, requirements))
        {
            return;
        }

        throw ErrorDefinition.Access().AsException();
    }

    /// <summary>
    /// Require the principal to have any of the defined scopes
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <returns></returns>
    public async Task RequireScopesAll(HttpContext context, IEnumerable<string> requirements)
    {
        // if access check passes just return
        if (await this.CheckScopesAll(context, requirements))
        {
            return;
        }

        throw ErrorDefinition.Access().AsException();
    }

    /// <summary>
    /// Requires the principal in the context has minimum user type
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="minUserType">The minimum user type</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    public async Task RequireMinUserType(HttpContext context, string minUserType, IEnumerable<string> allowedScopes)
    {
        // if access check passes just return
        if (await this.CheckMinUserType(context, minUserType, allowedScopes))
        {
            return;
        }

        throw ErrorDefinition.Access().AsException();
    }

    /// <summary>
    /// Require the principal to have any of the defined accesses
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    public Task<bool> CheckAccessAny(HttpContext context, IEnumerable<string> requirements, IEnumerable<string> allowedScopes)
    {
        // do the authorization request and check if access is allowed
        var result = AccessAuthorization.CheckAccess(context, requirements.ToHashSet(), false, allowedScopes.ToHashSet());

        return Task.FromResult(result);
    }

    /// <summary>
    /// Require the principal to have any of the defined accesses
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    public Task<bool> CheckAccessAll(HttpContext context, IEnumerable<string> requirements, IEnumerable<string> allowedScopes)
    {
        // do the authorization request and check if access is allowed
        var result = AccessAuthorization.CheckAccess(context, requirements.ToHashSet(), true, allowedScopes.ToHashSet());

        return Task.FromResult(result);
    }

    /// <summary>
    /// Check if the principal to have any of the defined scopes
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <returns></returns>
    public Task<bool> CheckScopesAny(HttpContext context, IEnumerable<string> requirements)
    {
        // do the authorization request and check if any scope access is allowed
        var result = ScopeAuthorization.CheckScopes(context, requirements.ToHashSet(), false);

        return Task.FromResult(result);
    }

    /// <summary>
    /// Checks if the principal to have any of the defined scopes
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <returns></returns>
    public Task<bool> CheckScopesAll(HttpContext context, IEnumerable<string> requirements)
    {
        // do the authorization request and check if all scope access is allowed
        var result = ScopeAuthorization.CheckScopes(context, requirements.ToHashSet(), true);

        return Task.FromResult(result);
    }

    /// <summary>
    /// Checks if the principal in the context has minimum user type
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="minUserType">The minimum user type</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    public Task<bool> CheckMinUserType(HttpContext context, string minUserType, IEnumerable<string> allowedScopes)
    {
        // do the authorization request to check if user has minimum required type
        var result = UserTypeAuthorization.CheckType(context, minUserType, allowedScopes.ToHashSet());

        return Task.FromResult(result);
    }
}