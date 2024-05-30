using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Privileges;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The privilege access repository.
/// </summary>
public interface IPrivilegeAccessRepository
{
    /// <summary>
    /// Gets all privilege access definitions.
    /// </summary>
    /// <returns>A task containing the privilege accesses.</returns>
    Task<IEnumerable<PrivilegeAccessModel>> GetAll();

    /// <summary>
    /// Gets privilege access definition by id.
    /// </summary>
    /// <param name="id">The id of the privilege access definition.</param>
    /// <returns>A task containing the privilege access definition.</returns>
    Task<PrivilegeAccessModel> GetById(string id);

    /// <summary>
    /// Get all privilege access definitions by privilege id.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <returns>A task containing privilege accesses.</returns>
    Task<IEnumerable<PrivilegeAccessModel>> GetByPrivilegeId(string privilegeId);

    /// <summary>
    /// Update privilege access based on the input.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <param name="input">The privilege access model update input</param>
    /// <returns>A task containing the privilege access update result.</returns>
    Task<PrivilegeAccessUpdateResultModel> UpdateByPrivilegeId(string privilegeId, PrivilegeAccessUpdateModel input);

    /// <summary>
    /// Delete privilege access grant by identifier.
    /// </summary>
    /// <param name="id">The privilege access declaration id.</param>
    /// <returns>A task containing the deleted privilege access.</returns>
    Task<PrivilegeAccessModel> DeleteById(string id);
}
