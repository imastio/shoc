using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.User;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The user access repository.
/// </summary>
public class UserAccessRepository : IUserAccessRepository
{
    /// <summary>
    /// The data operations instance.
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of user access grant repository implementation.
    /// </summary>
    /// <param name="dataOps">A DataOps instance.</param>
    public UserAccessRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets the all user access definitions.
    /// </summary>
    /// <returns>A task containing all user accesses.</returns>
    public Task<IEnumerable<UserAccessModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.User.Access", "GetAll").ExecuteAsync<UserAccessModel>();
    }

    /// <summary>
    /// Gets user access definition by id.
    /// </summary>
    /// <param name="id">The id of definition.</param>
    /// <returns>A task containing the user access definition.</returns>
    public Task<UserAccessModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Access", "GetById").ExecuteAsync<UserAccessModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Gets all user access definition by user id.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <returns>A task containing the user access definitions.</returns>
    public Task<IEnumerable<UserAccessModel>> GetByUserId(string userId)
    {
        return this.dataOps.Connect().Query("Identity.User.Access", "GetByUserId").ExecuteAsync<UserAccessModel>(new
        {
            Id = userId
        });
    }

    /// <summary>
    /// Update user access based on the input.
    /// </summary>
    /// <param name="userId">The user id.</param>
    /// <param name="input">The access model update input.</param>
    /// <returns>A task containing the user access update result.</returns>
    public async Task<UserAccessUpdateResultModel> UpdateByUserId(string userId, UserAccessUpdateModel input)
    {
        // get the revokation list
        var toRevoke = new HashSet<string>(input.Revoke ?? Enumerable.Empty<string>());

        // add all the grants to revokation list first
        toRevoke.UnionWith(input.Grant ?? new List<string>());

        // revoke the accesses 
        var revoked = await this.dataOps.Connect().QueryFirst("Identity.User.Access", "DeleteByAccess").ExecuteAsync<int>(new
        {
            UserId = userId,
            Revoke = toRevoke
        });

        // accesses to insert
        var toGrant = (input.Grant ?? new List<string>()).Select(access => new UserAccessModel
        {
            Id = StdIdGenerator.Next(IdentityObjects.UACC),
            UserId = userId,
            Access = access
        });

        // create objects to grant access
        await this.dataOps.Connect().NonQuery("Identity.User.Access", "Create").ExecuteAsync(toGrant);

        // return update result
        return new UserAccessUpdateResultModel
        {
            Granted = input.Grant?.Count ?? 0,
            Revoked = revoked
        };
    }

    /// <summary>
    /// Deletes user access grant by identifier.
    /// </summary>
    /// <param name="id">The user access declaration id</param>
    /// <returns>A task containing the deleted user acess definition.</returns>
    public Task<UserAccessModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.User.Access", "DeleteById").ExecuteAsync<UserAccessModel>(new
        {
            Id = id,
        });
    }
}
