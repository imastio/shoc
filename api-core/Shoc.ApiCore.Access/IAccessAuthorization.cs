using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shoc.ApiCore.Access;

/// <summary>
/// The access authorization service
/// </summary>
public interface IAccessAuthorization
{
    /// <summary>
    /// Require the principal to have any of the defined accesses
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    Task RequireAccessAny(HttpContext context, IEnumerable<string> requirements, IEnumerable<string> allowedScopes);
    
    /// <summary>
    /// Require the principal to have any of the defined accesses
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    Task RequireAccessAll(HttpContext context, IEnumerable<string> requirements, IEnumerable<string> allowedScopes);

    /// <summary>
    /// Require the principal to have any of the defined scopes
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <returns></returns>
    Task RequireScopesAny(HttpContext context, IEnumerable<string> requirements);
    
    /// <summary>
    /// Require the principal to have any of the defined scopes
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <returns></returns>
    Task RequireScopesAll(HttpContext context, IEnumerable<string> requirements);
    
    /// <summary>
    /// Requires the principal in the context has minimum user type
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="minUserType">The minimum user type</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    public Task RequireMinUserType(HttpContext context, string minUserType, IEnumerable<string> allowedScopes);
    
    /// <summary>
    /// Require the principal to have any of the defined accesses
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    Task<bool> CheckAccessAny(HttpContext context, IEnumerable<string> requirements, IEnumerable<string> allowedScopes);
    
    /// <summary>
    /// Require the principal to have any of the defined accesses
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    Task<bool> CheckAccessAll(HttpContext context, IEnumerable<string> requirements, IEnumerable<string> allowedScopes);

    /// <summary>
    /// Check if the principal to have any of the defined scopes
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <returns></returns>
    Task<bool> CheckScopesAny(HttpContext context, IEnumerable<string> requirements);
    
    /// <summary>
    /// Checks if the principal to have any of the defined scopes
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="requirements">The set of requirements</param>
    /// <returns></returns>
    Task<bool> CheckScopesAll(HttpContext context, IEnumerable<string> requirements);
    
    /// <summary>
    /// Checks if the principal in the context has minimum user type
    /// </summary>
    /// <param name="context">The http context</param>
    /// <param name="minUserType">The minimum user type</param>
    /// <param name="allowedScopes">The scopes allowed to bypass</param>
    /// <returns></returns>
    Task<bool> CheckMinUserType(HttpContext context, string minUserType, IEnumerable<string> allowedScopes);
}