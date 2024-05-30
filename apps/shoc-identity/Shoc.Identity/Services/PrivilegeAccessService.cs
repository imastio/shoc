using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Privileges;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The privilege access service.
/// </summary>
public class PrivilegeAccessService : PrivilegeServiceBase
{
    /// <summary>
    /// The privilege access repository.
    /// </summary>
    private readonly IPrivilegeAccessRepository privilegeAccessRepository;

    /// <summary>
    /// Creates the instance of privilege access service.
    /// </summary>
    /// <param name="privilegeRepository">The privilege repository.</param>
    /// <param name="privilegeAccessRepository">The privilege access repository</param>
    public PrivilegeAccessService(IPrivilegeRepository privilegeRepository, IPrivilegeAccessRepository privilegeAccessRepository) : base(privilegeRepository)
    {
        this.privilegeAccessRepository = privilegeAccessRepository;
    }

    /// <summary>
    /// Gets all the privilege accesses granted to the privilege by the privilege id.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <returns>A task containing all privilege accesses.</returns>
    public async Task<IEnumerable<PrivilegeAccessModel>> Get(string privilegeId)
    {
        // the privilege must exist
        await this.RequireById(privilegeId);

        // get all accesses of the privilege.
        return await this.privilegeAccessRepository.GetByPrivilegeId(privilegeId);
    }

    /// <summary>
    /// Updates the privilege accesses of the given privilege by privilege id and given input.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <param name="input">The privilege access update input.</param>
    /// <returns>A task containing the privilege access update result.</returns>
    public async Task<PrivilegeAccessUpdateResultModel> Update(string privilegeId, PrivilegeAccessUpdateModel input)
    {
        // the privilege must exist
        await this.RequireById(privilegeId);

        // perform the update operation
        return await this.privilegeAccessRepository.UpdateByPrivilegeId(privilegeId, input);
    }
}
