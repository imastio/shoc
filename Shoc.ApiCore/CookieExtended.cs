using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore
{
    /// <summary>
    /// The cookie configuration extensions
    /// </summary>
    public static class CookieExtended
    {
        /// <summary>
        /// Add the same-site cookie policy
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <returns></returns>
        public static IServiceCollection AddSameSiteCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.Secure = CookieSecurePolicy.Always;
                options.OnAppendCookie = ctx => HandleSameSite(ctx.Context, ctx.CookieOptions);
                options.OnDeleteCookie = ctx => HandleSameSite(ctx.Context, ctx.CookieOptions);
            });

            return services;
        }
        
        /// <summary>
        /// Handles the same site cookie
        /// </summary>
        /// <param name="httpContext">The http context</param>
        /// <param name="options">The options</param>
        private static void HandleSameSite(HttpContext httpContext, CookieOptions options)
        {
            // only in case of same site
            if (options.SameSite != SameSiteMode.None)
            {
                return;
            }
            
            // gets the user agent
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
            
            // check if not https or user agent does not allow same site cookie leave unspecified
            if (!httpContext.Request.IsHttps || DisallowsSameSiteNone(userAgent))
            {
                options.SameSite = SameSiteMode.Unspecified;
            }
        }
        
        /// <summary>
        /// Checks if user-agent disallows same-site none cookies
        /// </summary>
        /// <param name="userAgent">The user agent</param>
        /// <returns></returns>
        private static bool DisallowsSameSiteNone(string userAgent)
        {
            // Cover all iOS based browsers here. This includes:
            // - Safari on iOS 12 for iPhone, iPod Touch, iPad
            // - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            // - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the iOS networking stack
            if (userAgent.Contains("CPU iPhone OS 12") || userAgent.Contains("iPad; CPU OS 12"))
            {
                return true;
            }

            // Cover Mac OS X based browsers that use the Mac OS networking stack. This includes:
            // - Safari on Mac OS X.
            // This does not include:
            // - Chrome on Mac OS X
            // Because they do not use the Mac OS networking stack.
            if (userAgent.Contains("Macintosh; Intel Mac OS X 10_14") &&
                userAgent.Contains("Version/") && userAgent.Contains("Safari"))
            {
                return true;
            }

            // Cover Chrome 50-69, because some versions are broken by SameSite=None,
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions,
            // but pre-Chromium Edge does not require SameSite=None.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
                return true;
            }

            return false;
        }
    }
}