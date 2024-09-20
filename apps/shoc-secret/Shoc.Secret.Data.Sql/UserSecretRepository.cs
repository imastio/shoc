using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Secret.Model;
using Shoc.Secret.Model.UserSecret;

namespace Shoc.Secret.Data.Sql;

/// <summary>
/// The repository implementation
/// </summary>
public class UserSecretRepository : IUserSecretRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public UserSecretRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<UserSecretModel>> GetAll(string workspaceId, string userId)
    {
        return this.dataOps.Connect().Query("Secret.UserSecret", "GetAll").ExecuteAsync<UserSecretModel>(new
        {
            WorkspaceId = workspaceId,
            UserId = userId
        });
    }

    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<UserSecretExtendedModel>> GetAllExtended(string workspaceId, string userId)
    {
        return this.dataOps.Connect().Query("Secret.UserSecret", "GetAllExtended").ExecuteAsync<UserSecretExtendedModel>(new
        {
            WorkspaceId = workspaceId,
            UserId = userId 
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<UserSecretModel> GetById(string workspaceId, string userId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Secret.UserSecret", "GetById").ExecuteAsync<UserSecretModel>(new
        {
            WorkspaceId = workspaceId,
            UserId = userId,
            Id = id
        });
    }

    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <returns></returns>
    public Task<UserSecretModel> GetByName(string workspaceId, string userId, string name)
    {
        return this.dataOps.Connect().QueryFirst("Secret.UserSecret", "GetByName").ExecuteAsync<UserSecretModel>(new
        {
            WorkspaceId = workspaceId,
            UserId = userId,
            Name = name
        });
    }

    /// <summary>
    /// Count objects by workspace id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public Task<UserSecretCountModel> CountAll(string workspaceId, string userId)
    {
        return this.dataOps.Connect().QueryFirst("Secret.UserSecret", "CountAll").ExecuteAsync<UserSecretCountModel>(new
        {
            WorkspaceId = workspaceId,
            UserId = userId
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<UserSecretModel> Create(string workspaceId, string userId, UserSecretCreateModel input)
    {
        // assign the id
        input.WorkspaceId = workspaceId;
        input.UserId = userId;
        input.Id ??= StdIdGenerator.Next(SecretObjects.USER_SECRET).ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Secret.UserSecret", "Create").ExecuteAsync<UserSecretModel>(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<UserSecretModel> UpdateById(string workspaceId, string userId, string id, UserSecretUpdateModel input)
    {
        input.WorkspaceId = workspaceId;
        input.UserId = userId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Secret.UserSecret", "UpdateById").ExecuteAsync<UserSecretModel>(input);
    }

    /// <summary>
    /// Updates the object value by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<UserSecretModel> UpdateValueById(string workspaceId, string userId, string id, UserSecretValueUpdateModel input)
    {
        input.WorkspaceId = workspaceId;
        input.UserId = userId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Secret.UserSecret", "UpdateValueById").ExecuteAsync<UserSecretModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<UserSecretModel> DeleteById(string workspaceId, string userId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Secret.UserSecret", "DeleteById").ExecuteAsync<UserSecretModel>(new
        {
            WorkspaceId = workspaceId,
            UserId = userId,
            Id = id
        });
    }
}