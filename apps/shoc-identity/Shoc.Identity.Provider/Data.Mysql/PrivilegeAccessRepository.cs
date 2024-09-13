using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Privileges;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The privilege access repository.
/// </summary>
public class PrivilegeAccessRepository : IPrivilegeAccessRepository
{
    /// <summary>
    /// The data operations instance.
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of privilege access grant repository implementation.
    /// </summary>
    /// <param name="dataOps">A DataOps instance.</param>
    public PrivilegeAccessRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all privilege accesses.
    /// </summary>
    /// <returns>A task containing all privilege access definitions.</returns>
    public Task<IEnumerable<PrivilegeAccessModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.Privilege.Access", "GetAll").ExecuteAsync<PrivilegeAccessModel>();
    }

    /// <summary>
    /// Get all privilege access definitions by privilege id.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <returns>A task containing privilege accesses.</returns>
    public Task<IEnumerable<PrivilegeAccessModel>> GetByPrivilegeId(string privilegeId)
    {
        return this.dataOps.Connect().Query("Identity.Privilege.Access", "GetByPrivilegeId").ExecuteAsync<PrivilegeAccessModel>(new
        {
            Id = privilegeId
        });
    }
 
    /// <summary>
    /// Gets the privilege access by id.
    /// </summary>
    /// <param name="id">The id of privilege access definition.</param>
    /// <returns>A task containing the privilege access.</returns>
    public Task<PrivilegeAccessModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Privilege.Access", "GetById").ExecuteAsync<PrivilegeAccessModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Update privilege access based on the input.
    /// </summary>
    /// <param name="privilegeId">The privilege id.</param>
    /// <param name="input">The privilege access update model.</param>
    /// <returns>A task containing the privilege access update result.</returns>
    public async Task<PrivilegeAccessUpdateResultModel> UpdateByPrivilegeId(string privilegeId, PrivilegeAccessUpdateModel input)
    {
        // get the revokation list
        var toRevoke = new HashSet<string>(input.Revoke ?? Enumerable.Empty<string>());

        // add all the grants to revokation list first
        toRevoke.UnionWith(input.Grant ?? new List<string>());

        // revoke the accesses 
        var revoked = await this.dataOps.Connect().QueryFirst("Identity.Privilege.Access", "DeleteByAccess").ExecuteAsync<int>(new
        {
            PrivilegeId = privilegeId,
            Revoke = toRevoke
        });

        // accesses to insert
        var toGrant = (input.Grant ?? new List<string>()).Select(access => new PrivilegeAccessModel
        {
            Id = StdIdGenerator.Next(IdentityObjects.PACC),
            PrivilegeId = privilegeId,
            Access = access
        });

        // create objects to grant access
        await this.dataOps.Connect().NonQuery("Identity.Privilege.Access", "Create").ExecuteAsync(toGrant);

        // return update result
        return new PrivilegeAccessUpdateResultModel
        {
            Granted = input.Grant?.Count ?? 0,
            Revoked = revoked
        };
    }

    /// <summary>
    /// Deletes the privilege access by id.
    /// </summary>
    /// <param name="id">The privilege access id.</param>
    /// <returns>The task containing the deleted privilege access.</returns>
    public Task<PrivilegeAccessModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Privilege.Access", "DeleteById").ExecuteAsync<PrivilegeAccessModel>(new
        {
            Id = id
        });
    }
}
