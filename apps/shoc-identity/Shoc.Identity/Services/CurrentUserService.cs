using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Core.Identity;

namespace Shoc.Identity.Services;

/// <summary>
/// The current user service
/// </summary>
public class CurrentUserService
{
    /// <summary>
    /// The user access service
    /// </summary>
    private readonly UserAccessService userAccessService;

    /// <summary>
    /// Creates new instance of current user service
    /// </summary>
    /// <param name="userAccessService">The user access service</param>
    public CurrentUserService(UserAccessService userAccessService)
    {
        this.userAccessService = userAccessService;
    }

    /// <summary>
    /// Gets the effective access list
    /// </summary>
    /// <param name="principal">The current principal</param>
    /// <returns></returns>
    public async Task<IEnumerable<string>> GetEffectiveAccesses(ShocPrincipal principal)
    {
        // get the access models assigned to the user
        var accesses = await this.userAccessService.GetEffective(principal.Id);

        // return access modifiers only
        return accesses.Select(access => access.Access).ToHashSet();
    }
}