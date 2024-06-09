using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shoc.Identity.Services;

/// <summary>
/// The middleware to handle non-api SPA client-routing requests
/// </summary>
public class NonApiSpaMiddleware
{
    /// <summary>
    /// The set of known API path prefixes
    /// </summary>
    private static readonly ISet<string> API_PATHS = new HashSet<string> { "/api", "/api-auth", "/connect", "/.well-known" };
    
    /// <summary>
    /// The next request delegate
    /// </summary>
    private readonly RequestDelegate next;
    
    /// <summary>
    /// Creates new instance of middleware
    /// </summary>
    /// <param name="next">The next delegate</param>
    public NonApiSpaMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// The implementation of middleware logic
    /// </summary>
    /// <param name="context">The current http context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        // the request path
        var path = context.Request.Path;

        // indicate if API request
        var apiRequest = API_PATHS.Any(apiPath => path.StartsWithSegments(apiPath));
        
        // indicate if file request
        var fileRequest = Path.HasExtension(path.Value);
        
        // if not an API nor file request fallback to index.html
        if (!apiRequest && !fileRequest)
        {
            context.Request.Path = "/index.html";
        }
        
        // move on
        await this.next(context);
    }
}