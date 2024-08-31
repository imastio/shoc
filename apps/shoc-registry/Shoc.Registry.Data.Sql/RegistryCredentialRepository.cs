using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Credential;

namespace Shoc.Registry.Data.Sql;

/// <summary>
/// The repository credential implementation
/// </summary>
public class RegistryCredentialRepository : IRegistryCredentialRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public RegistryCredentialRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    public Task<IEnumerable<RegistryCredentialModel>> GetAll(string registryId)
    {
        return this.dataOps.Connect().Query("Registry.Credential", "GetBy").ExecuteAsync<RegistryCredentialModel>(new
        {
            RegistryId = registryId
        });
    }

    /// <summary>
    /// Gets all the objects with filter
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="filter">The filter to apply</param>
    /// <returns></returns>
    public Task<IEnumerable<RegistryCredentialModel>> GetBy(string registryId, RegistryCredentialFilter filter)
    {
        return this.dataOps.Connect().Query("Registry.Credential", "GetBy")
            .WithBinding("ByWorkspace", filter.ByWorkspace && !string.IsNullOrWhiteSpace(filter.WorkspaceId))
            .WithBinding("ByWorkspaceNull", filter.ByWorkspace && string.IsNullOrWhiteSpace(filter.WorkspaceId))
            .WithBinding("ByUser", filter.ByUser && !string.IsNullOrWhiteSpace(filter.UserId))
            .WithBinding("ByUserNull", filter.ByUser && string.IsNullOrWhiteSpace(filter.UserId))
            .WithBinding("ByPullAllowed", filter.PullAllowed.HasValue)
            .WithBinding("ByPushAllowed", filter.PushAllowed.HasValue)
            .ExecuteAsync<RegistryCredentialModel>(new
        {
            RegistryId = registryId,
            filter.WorkspaceId,
            filter.UserId,
            filter.PullAllowed,
            filter.PushAllowed
        });
    }

    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    public Task<IEnumerable<RegistryCredentialExtendedModel>> GetAllExtended(string registryId)
    {
        return this.dataOps.Connect().Query("Registry.Credential", "GetExtendedBy").ExecuteAsync<RegistryCredentialExtendedModel>(new
        {
            RegistryId = registryId
        });
    }

    /// <summary>
    /// Gets all the extended objects by filter
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="filter">The filter to apply</param>
    /// <returns></returns>
    public Task<IEnumerable<RegistryCredentialExtendedModel>> GetExtendedBy(string registryId, RegistryCredentialFilter filter)
    {
        return this.dataOps.Connect().Query("Registry.Credential", "GetExtendedBy")
            .WithBinding("ByWorkspace", filter.ByWorkspace && !string.IsNullOrWhiteSpace(filter.WorkspaceId))
            .WithBinding("ByWorkspaceNull", filter.ByWorkspace && string.IsNullOrWhiteSpace(filter.WorkspaceId))
            .WithBinding("ByUser", filter.ByUser && !string.IsNullOrWhiteSpace(filter.UserId))
            .WithBinding("ByUserNull", filter.ByUser && string.IsNullOrWhiteSpace(filter.UserId))
            .WithBinding("ByPullAllowed", filter.PullAllowed.HasValue)
            .WithBinding("ByPushAllowed", filter.PushAllowed.HasValue)
            .ExecuteAsync<RegistryCredentialExtendedModel>(new
        {
            RegistryId = registryId,
            filter.WorkspaceId,
            filter.UserId,
            filter.PullAllowed,
            filter.PushAllowed
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public Task<RegistryCredentialModel> GetById(string registryId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Registry.Credential", "GetById").ExecuteAsync<RegistryCredentialModel>(new
        {
            RegistryId = registryId,
            Id = id
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<RegistryCredentialModel> Create(RegistryCredentialCreateModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(RegistryObjects.REGISTRY_CREDENTIAL)?.ToLowerInvariant();

        // perform operation
        return this.dataOps.Connect().QueryFirst("Registry.Credential", "Create").ExecuteAsync<RegistryCredentialModel>(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<RegistryCredentialModel> UpdateById(string registryId, string id, RegistryCredentialUpdateModel input)
    {
        input.RegistryId = registryId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Registry.Credential", "UpdateById").ExecuteAsync<RegistryCredentialModel>(input);
    }

    /// <summary>
    /// Updates the object password by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<RegistryCredentialModel> UpdatePasswordById(string registryId, string id, RegistryCredentialPasswordUpdateModel input)
    {
        input.RegistryId = registryId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Registry.Credential", "UpdatePasswordById").ExecuteAsync<RegistryCredentialModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<RegistryCredentialModel> DeleteById(string registryId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Registry.Credential", "DeleteById").ExecuteAsync<RegistryCredentialModel>(new
        {
            RegistryId = registryId,
            Id = id
        });
    }
}