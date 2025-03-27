using Microsoft.AspNetCore.Http;

namespace Shoc.Identity.Config.Oidc;

/// <summary>
/// The openid provider accessor
/// </summary>
public class DynamicOidcProviderAccessor
{
    /// <summary>
    /// The http context accessor
    /// </summary>
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Creates new instance of the accessor
    /// </summary>
    /// <param name="httpContextAccessor">The context accessor</param>
    public DynamicOidcProviderAccessor(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Gets the current openid tenant
    /// </summary>
    /// <returns></returns>
    public string Get()
    {
        // get the http context
        var context = this.httpContextAccessor.HttpContext;

        // no context
        if (context == null)
        {
            return null;
        }

        // handle if callback path from external provider
        if (context.Request.Path.StartsWithSegments(OidcProviderConstants.CALLBACK_PATH))
        {
            return context.Request.Path.Value?.Replace($"{OidcProviderConstants.CALLBACK_PATH}/", string.Empty);
        }

        // try getting provider code
        return context.Request.RouteValues.TryGetValue(OidcProviderConstants.PROVIDER_CODE_KEY, out var res) ? res?.ToString() : null;
    }
}
