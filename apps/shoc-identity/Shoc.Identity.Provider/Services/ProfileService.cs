using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Shoc.Core.OpenId;
using Shoc.Identity.Model.User;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Provider.Services;

/// <summary>
/// The identity profile service 
/// </summary>
public class ProfileService : IProfileService
{
    /// <summary>
    /// The user repository
    /// </summary>
    private readonly IUserInternalRepository userInternalRepository;

    /// <summary>
    /// Creates new instance of profile service
    /// </summary>
    /// <param name="userInternalRepository">The user internal repository</param>
    public ProfileService(IUserInternalRepository userInternalRepository)
    {
        this.userInternalRepository = userInternalRepository;
    }

    /// <summary>
    /// Get the profile data 
    /// </summary>
    /// <param name="context">The request context</param>
    /// <returns></returns>
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        // get the identifier of the user
        var id = context.Subject.FindFirst(KnownClaims.SUBJECT)?.Value ?? string.Empty;

        // gets the user by subject id
        var user = string.IsNullOrEmpty(id) ? null : await this.userInternalRepository.GetById(id);

        // resulting claims
        var claims = new List<Claim>();
            
        // get requested claims + required ones
        var requestedClaims = context.RequestedClaimTypes.ToHashSet();

        // process each claim type
        foreach (var claimType in requestedClaims)
        {
            // add claims of the given type extracted from the user
            claims.AddRange(GetUserClaims(user, claimType));
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
        var id = context.Subject.FindFirst(KnownClaims.SUBJECT)?.Value ?? string.Empty;

        // gets the user by subject id
        var user = await this.userInternalRepository.GetById(id);

        // user can obtain token if is valid
        context.IsActive = string.IsNullOrEmpty(id) || user is { Deleted: false };
    }

    /// <summary>
    /// Gets the generic claims by type
    /// </summary>
    /// <param name="user">The user entity</param>
    /// <param name="claimType">The claim type</param>
    /// <returns></returns>
    private static IEnumerable<Claim> GetUserClaims(UserInternalModel user, string claimType)
    {
        return claimType switch
        {
            KnownClaims.SUBJECT => new[] { new Claim(claimType, user.Id) },
            KnownClaims.ID => new[] {new Claim(claimType, user.Id)},
            KnownClaims.EMAIL => user.Email.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.Email) },
            KnownClaims.EMAIL_VERIFIED => new[] { new Claim(claimType, user.EmailVerified.ToString(), ClaimValueTypes.Boolean) },
            KnownClaims.PREFERRED_USERNAME => user.Username.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.Username) },
            KnownClaims.NAME => user.FullName.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.FullName) },
            KnownClaims.PHONE_NUMBER => user.Phone.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.Phone) },
            KnownClaims.PHONE_NUMBER_VERIFIED => new[] { new Claim(claimType, user.PhoneVerified.ToString(), ClaimValueTypes.Boolean) },
            KnownClaims.USER_TYPE => user.Type.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.Type) },
            KnownClaims.ZONE_INFO => user.Timezone.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.Timezone) },
            KnownClaims.GIVEN_NAME => user.FirstName.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.FirstName) },
            KnownClaims.FAMILY_NAME => user.LastName.IsNullOrEmpty() ? Enumerable.Empty<Claim>() : new[] { new Claim(claimType, user.LastName) },
            _ => Enumerable.Empty<Claim>()
        };
    }
}