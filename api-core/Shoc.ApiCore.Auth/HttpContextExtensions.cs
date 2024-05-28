using System.Collections.Generic;
using Shoc.Core;
using Microsoft.AspNetCore.Http;
using Shoc.Core.Identity;
using Shoc.Core.OpenId;

namespace Shoc.ApiCore.Auth;

/// <summary>
/// The http context extensions
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// The empty accesses
    /// </summary>
    private static readonly ISet<string> EMPTY_ACCESSES = new HashSet<string>();
    
    /// <summary>
    /// Gets the principal object
    /// </summary>
    /// <param name="httpContext">The http context</param>
    /// <returns></returns>
    public static ShocPrincipal GetPrincipal(this HttpContext httpContext)
    {
        // get the id from claim
        var subject = httpContext.User.FindFirst(claim => claim.Type == KnownClaims.SUBJECT)?.Value;

        // make sure subject exists
        if (subject == null)
        {
            throw ErrorDefinition.Access().AsException();
        }

        return new ShocPrincipal
        {
            Id = subject,
            Accesses =  httpContext.GetAccesses()
        };
    }

    /// <summary>
    /// Gets accesses from the http context
    /// </summary>
    /// <param name="httpContext">The http context</param>
    /// <returns></returns>
    public static ISet<string> GetAccesses(this HttpContext httpContext)
    {
        return httpContext.GetItemOrDefault("Accesses", EMPTY_ACCESSES);
    }
}