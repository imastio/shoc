using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.UserGroup;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The user group repository implementation
/// </summary>
public class UserGroupRepository : IUserGroupRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of user groups repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public UserGroupRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the user groups
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<UserGroupModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.UserGroup", "GetAll").ExecuteAsync<UserGroupModel>();
    }

    /// <summary>
    /// Gets the user group by id
    /// </summary>
    /// <param name="id">The user group id</param>
    /// <returns></returns>
    public Task<UserGroupModel> GetById(string id)
    {
        // no user do not even try
        if (string.IsNullOrWhiteSpace(id))
        {
            return Task.FromResult(default(UserGroupModel));
        }

        // try load from database
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup", "GetById").ExecuteAsync<UserGroupModel>(new
        {
            Id = id.ToLowerInvariant()
        });
    }

    /// <summary>
    /// Gets the user group by name
    /// </summary>
    /// <param name="name">The user group name</param>
    /// <returns></returns>
    public Task<UserGroupModel> GetByName(string name)
    {
        // empty names are not allowed
        if (string.IsNullOrWhiteSpace(name))
        {
            return Task.FromResult(default(UserGroupModel));
        }

        // try load from database
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup", "GetByName").ExecuteAsync<UserGroupModel>(new
        {
            Name = name
        });
    }

    /// <summary>
    /// Creates entity based on input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<UserGroupModel> Create(UserGroupCreateModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(IdentityObjects.USER_GROUP)?.ToLowerInvariant();

        // add user group to the database
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup", "Create").ExecuteAsync<UserGroupModel>(input);
    }

    /// <summary>
    /// Updates the user group by given input.
    /// </summary>
    /// <param name="input">The user group update input.</param>
    /// <returns></returns>
    public Task<UserGroupModel> UpdateById(UserGroupUpdateModel input)
    {
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup", "UpdateById").ExecuteAsync<UserGroupModel>(input);
    }

    /// <summary>
    /// Deletes the user group by id
    /// </summary>
    /// <param name="id">The user group id</param>
    /// <returns></returns>
    public Task<UserGroupModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.UserGroup", "DeleteById").ExecuteAsync<UserGroupModel>(new
        {
            Id = id?.ToLowerInvariant()
        });
    }
}
