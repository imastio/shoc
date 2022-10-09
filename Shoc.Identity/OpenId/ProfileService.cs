using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Shoc.Identity.Data;
using Shoc.Identity.Model;

namespace Shoc.Identity.OpenId
{
    /// <summary>
    /// The identity profile service 
    /// </summary>
    public class ProfileService : IProfileService
    {
        /// <summary>
        /// The set of access token claims allowed
        /// </summary>
        private static readonly ISet<string> ACCESS_TOKEN_CLAIMS = new HashSet<string>
        {
            KnownClaims.SUBJECT,
            KnownClaims.EMAIL,
            KnownClaims.EMAIL_VERIFIED,
            KnownClaims.PREFERRED_USERNAME,
            KnownClaims.NAME,
            KnownClaims.USER_TYPE
        };

        /// <summary>
        /// The set of access token claims allowed
        /// </summary>
        private static readonly ISet<string> ID_TOKEN_CLAIMS = new HashSet<string>(ACCESS_TOKEN_CLAIMS);

        /// <summary>
        /// The set of access token claims allowed
        /// </summary>
        private static readonly ISet<string> USER_PROFILE_CLAIMS = new HashSet<string>(ID_TOKEN_CLAIMS);

        /// <summary>
        /// Required claims in case if even not requested
        /// </summary>
        private static readonly ISet<string> REQUIRED_CLAIMS = new HashSet<string> { JwtClaimTypes.Subject, JwtClaimTypes.Role };

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Creates new instance of profile service
        /// </summary>
        /// <param name="userRepository">The user repository</param>
        public ProfileService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Get the profile data 
        /// </summary>
        /// <param name="context">The request context</param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // get the identifier of the user
            var id = context.Subject.FindFirst(JwtClaimTypes.Subject)?.Value ?? string.Empty;

            // gets the user by subject id
            var user = await this.userRepository.GetById(id);

            // no user
            if (user == null)
            {
                return;
            }

            // resulting claims
            var claims = new List<Claim>();

            // get requested claims + required ones
            var requestedClaims = context.RequestedClaimTypes.ToHashSet();

            // get allowed claim types
            var allowedClaimTypes = context.Caller switch
            {
                IdentityServerConstants.ProfileDataCallers.ClaimsProviderAccessToken => ACCESS_TOKEN_CLAIMS,
                IdentityServerConstants.ProfileDataCallers.ClaimsProviderIdentityToken => ID_TOKEN_CLAIMS,
                IdentityServerConstants.ProfileDataCallers.UserInfoEndpoint => USER_PROFILE_CLAIMS,
                _ => new HashSet<string>()
            };

            // filter requested claims with allowed ones, but add requires claims
            var finalClaims = requestedClaims.Intersect(allowedClaimTypes).Union(REQUIRED_CLAIMS);

            // process each claim type
            foreach (var type in finalClaims)
            {
                claims.AddRange(GetGenericClaims(user, type));
            }

            // set claims
            context.IssuedClaims = claims;
        }

        /// <summary>
        /// The active context detection
        /// </summary>
        /// <param name="context">The active context</param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            // get the identifier of the user
            var id = context.Subject.FindFirst(JwtClaimTypes.Subject)?.Value ?? string.Empty;

            // gets the user by subject id
            var user = await this.userRepository.GetById(id);

            // user can obtain token if is valid
            context.IsActive = user != null && !user.Deleted;
        }

        /// <summary>
        /// Gets the generic claims by type
        /// </summary>
        /// <param name="user">The user entity</param>
        /// <param name="claimType">The claim type</param>
        /// <returns></returns>
        private static IEnumerable<Claim> GetGenericClaims(UserModel user, string claimType)
        {
            return claimType switch
            {
                KnownClaims.SUBJECT => new[] { new Claim(claimType, user.Id) },
                KnownClaims.ID => new[] { new Claim(claimType, user.Id) },
                KnownClaims.EMAIL => user.Email.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.Email) },
                KnownClaims.EMAIL_VERIFIED => new[] { new Claim(claimType, user.EmailVerified.ToString(), ClaimValueTypes.Boolean) },
                KnownClaims.PREFERRED_USERNAME => user.Username.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.Username) },
                KnownClaims.NAME => user.FullName.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.FullName) },
                KnownClaims.USER_TYPE => user.Type.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.Type) },
                KnownClaims.GIVEN_NAME => user.FirstName.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.FirstName) },
                KnownClaims.FAMILY_NAME => user.LastName.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.LastName) },
                _ => Enumerable.Empty<Claim>()
            };
        }
    }
}