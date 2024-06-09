using Microsoft.AspNetCore.Http;

namespace Shoc.Identity.Config;

/// <summary>
/// The API route extensions
/// </summary>
public static class ApiRouteExtensions
{
    /// <summary>
    /// Checks if the context is from API call
    /// </summary>
    /// <param name="httpContext">The http context</param>
    /// <returns></returns>
    public static bool IsApiContext(this HttpContext httpContext)
    {
        if (httpContext.Request.Path.StartsWithSegments("/api"))
        {
            return true;
        }
        
        if (httpContext.Request.Path.StartsWithSegments("/api-auth"))
        {
            return true;
        }
        
        if (httpContext.Request.Path.StartsWithSegments("/connect"))
        {
            return true;
        }
        
        if (httpContext.Request.Path.StartsWithSegments("/.well-known"))
        {
            return true;
        }

        return false;
    }
}