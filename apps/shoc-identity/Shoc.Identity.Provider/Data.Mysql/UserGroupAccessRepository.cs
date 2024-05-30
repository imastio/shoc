using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.UserGroup;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The user group access repository.
/// </summary>
public class UserGroupAccessRepository : IUserGroupAccessRepository
{
    /// <summary>
    /// The data operations instance.
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of user group access grant repository implementation.
    /// </summary>
    /// <param name="dataOps">A DataOps instance.</param>
    public UserGroupAccessRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets the all user group access definitions.
    /// </summary>
    /// <returns>A task containing all user group accesses.</returns>
    public Task<IEnumerable<UserGroupAccessModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.UserGroup.Access", "GetAll").ExecuteAsync<UserGroupAccessModel>();
    }

    /// <summary>
    /// Gets user group access definition by id.
    /// </summary>
    /// <param name="id">The id of definition.</param>
    /// <returns>A task containing the user group access definition.</returns>
    public Task<UserGroupAccessModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup.Access", "GetById").ExecuteAsync<UserGroupAccessModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Gets all user group access definition by user id.
    /// </summary>
    /// <param name="groupId">The group id.</param>
    /// <returns>A task containing the user group access definitions.</returns>

    public Task<IEnumerable<UserGroupAccessModel>> GetByGroupId(string groupId)
    {
        return this.dataOps.Connect().Query("Identity.UserGroup.Access", "GetByGroupId").ExecuteAsync<UserGroupAccessModel>(new
        {
            Id = groupId
        });
    }

    /// <summary>
    /// Update user group access based on the input.
    /// </summary>
    /// <param name="groupId">The group id.</param>
    /// <param name="input">The access model update input.</param>
    /// <returns>A task containing the user group access update result.</returns>
    public async Task<UserGroupAccessUpdateResultModel> UpdateByGroupId(string groupId, UserGroupAccessUpdateModel input)
    {
        // get the revokation list
        var toRevoke = new HashSet<string>(input.Revoke ?? Enumerable.Empty<string>());

        // add all the grants to revokation list first
        toRevoke.UnionWith(input.Grant ?? new List<string>());

        // revoke the accesses 
        var revoked = await this.dataOps.Connect().QueryFirst("Identity.UserGroup.Access", "DeleteByAccess").ExecuteAsync<int>(new
        {
            GroupId = groupId,
            Revoke = toRevoke
        });

        // accesses to insert
        var toGrant = (input.Grant ?? new List<string>()).Select(access => new UserGroupAccessModel
        {
            Id = StdIdGenerator.Next(IdentityObjects.UGACC),
            GroupId = groupId,
            Access = access
        });

        // create objects to grant access
        await this.dataOps.Connect().NonQuery("Identity.UserGroup.Access", "Create").ExecuteAsync(toGrant);

        // return update result
        return new UserGroupAccessUpdateResultModel
        {
            Granted = input.Grant?.Count ?? 0,
            Revoked = revoked
        };
    }

    /// <summary>
    /// Deletes user group access grant by identifier.
    /// </summary>
    /// <param name="id">The user group access declaration id</param>
    /// <returns>A task containing the deleted user group acess definition.</returns>
    public Task<UserGroupAccessModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup.Access", "DeleteById").ExecuteAsync<UserGroupAccessModel>(new
        {
            Id = id
        });
    }
}
