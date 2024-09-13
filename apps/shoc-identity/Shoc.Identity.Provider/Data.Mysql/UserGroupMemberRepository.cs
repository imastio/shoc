using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Identity.Model.User;
using Shoc.Identity.Model.UserGroup;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The user group member repository
/// </summary>
public class UserGroupMemberRepository : IUserGroupMemberRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of user group members repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public UserGroupMemberRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the users in the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <returns></returns>
    public Task<IEnumerable<UserReferentialValueModel>> GetAll(string groupId)
    {
        return this.dataOps.Connect().Query("Identity.UserGroup.Member", "GetAll").ExecuteAsync<UserReferentialValueModel>(new
        {
            GroupId = groupId
        });
    }

    /// <summary>
    /// Gets the particular user in the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public Task<UserReferentialValueModel> GetById(string groupId, string userId)
    {
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup.Member", "GetById").ExecuteAsync<UserReferentialValueModel>(new
        {
            GroupId = groupId,
            UserId = userId
        });
    }

    /// <summary>
    /// Creates new membership record for the given
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<UserReferentialValueModel> Create(UserGroupMembership input)
    {
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup.Member", "Create").ExecuteAsync<UserReferentialValueModel>(new
        {
            input.GroupId,
            input.UserId
        });
    }

    /// <summary>
    /// Deletes the member user from the group
    /// </summary>
    /// <param name="groupId">The group id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public Task<UserReferentialValueModel> DeleteById(string groupId, string userId)
    {
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup.Member", "DeleteById").ExecuteAsync<UserReferentialValueModel>(new
        {
            GroupId = groupId,
            UserId = userId
        });
    }
}