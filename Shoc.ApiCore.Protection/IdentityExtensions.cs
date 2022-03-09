using System.Linq;
using Microsoft.AspNetCore.Http;
using Shoc.Identity.Model;
using Shoc.ModelCore;

namespace Shoc.ApiCore.Protection
{
    /// <summary>
    /// The identity related extensions
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Gets the shoc principal from authenticated user
        /// </summary>
        /// <param name="context">The context</param>
        /// <returns></returns>
        public static ShocPrincipal GetShocPrincipal(this HttpContext context)
        {
            // get the issued claims
            var claims = context.User.Claims.ToList();

            // get the user id
            var sub = claims.FirstOrDefault(c => c.Type == KnownClaims.SUBJECT)?.Value ?? string.Empty;

            // get the role claim
            var role = claims.FirstOrDefault(c => c.Type == KnownClaims.ROLE)?.Value ?? Roles.USER;

            // return the shoc principal
            return new ShocPrincipal
            {
                Subject = sub,
                Role = role 
            };
        }
    }
}