using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Secret.Model;
using Shoc.Secret.Model.Secret;

namespace Shoc.Secret.Data.Sql;

/// <summary>
/// The repository implementation
/// </summary>
public class SecretRepository : ISecretRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public SecretRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<SecretModel>> GetAll(string workspaceId)
    {
        return this.dataOps.Connect().Query("Secret", "GetAll").ExecuteAsync<SecretModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<SecretExtendedModel>> GetAllExtended(string workspaceId)
    {
        return this.dataOps.Connect().Query("Secret", "GetAllExtended").ExecuteAsync<SecretExtendedModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<SecretModel> GetById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Secret", "GetById").ExecuteAsync<SecretModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }

    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <returns></returns>
    public Task<SecretModel> GetByName(string workspaceId, string name)
    {
        return this.dataOps.Connect().QueryFirst("Secret", "GetByName").ExecuteAsync<SecretModel>(new
        {
            WorkspaceId = workspaceId,
            Name = name
        });
    }

    /// <summary>
    /// Count objects by workspace id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public Task<SecretCountModel> CountAll(string workspaceId)
    {
        return this.dataOps.Connect().QueryFirst("Secret", "CountAll").ExecuteAsync<SecretCountModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<SecretModel> Create(string workspaceId, SecretCreateModel input)
    {
        // assign the id
        input.WorkspaceId = workspaceId;
        input.Id ??= StdIdGenerator.Next(SecretObjects.SECRET).ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Secret", "Create").ExecuteAsync<SecretModel>(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<SecretModel> UpdateById(string workspaceId, string id, SecretUpdateModel input)
    {
        input.WorkspaceId = workspaceId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Secret", "UpdateById").ExecuteAsync<SecretModel>(input);
    }

    /// <summary>
    /// Updates the object value by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<SecretModel> UpdateValueById(string workspaceId, string id, SecretValueUpdateModel input)
    {
        input.WorkspaceId = workspaceId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Secret", "UpdateValueById").ExecuteAsync<SecretModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<SecretModel> DeleteById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Secret", "DeleteById").ExecuteAsync<SecretModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
}