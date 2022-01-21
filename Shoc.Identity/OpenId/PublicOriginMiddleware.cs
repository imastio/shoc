using System;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;

namespace Shoc.Identity.OpenId
{
    /// <summary>
    /// The identity server public origin middleware
    /// </summary>
    public class PublicOriginMiddleware : IMiddleware
    {
        /// <summary>
        /// The identity settings
        /// </summary>
        private readonly IdentitySettings settings;

        /// <summary>
        /// Creates new instance of public origin middleware
        /// </summary>
        /// <param name="settings">The settings</param>
        public PublicOriginMiddleware(IdentitySettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// The logic of middleware
        /// </summary>
        /// <param name="context">The http request context</param>
        /// <param name="next">The next action in the chain</param>
        /// <returns></returns>
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // parse given origin
            var uri = new Uri(settings.PublicOrigin);

            // set identity server origin
            context.SetIdentityServerOrigin($"{uri.Scheme}://{uri.Host}:{uri.Port}");

            // set base path
            context.SetIdentityServerBasePath(settings.BasePath);
           
            // invoke next in the chain
            return next(context);
        }
    }
}