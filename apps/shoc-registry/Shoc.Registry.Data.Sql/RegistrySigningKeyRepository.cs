using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Registry.Model;
using Shoc.Registry.Model.Key;

namespace Shoc.Registry.Data.Sql;

/// <summary>
/// The repository signing key implementation
/// </summary>
public class RegistrySigningKeyRepository : IRegistrySigningKeyRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public RegistrySigningKeyRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <returns></returns>
    public Task<IEnumerable<RegistrySigningKeyModel>> GetAll(string registryId)
    {
        return this.dataOps.Connect().Query("Registry.SigningKey", "GetAll").ExecuteAsync<RegistrySigningKeyModel>(new
        {
            RegistryId = registryId
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public Task<RegistrySigningKeyModel> GetById(string registryId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Registry.SigningKey", "GetById").ExecuteAsync<RegistrySigningKeyModel>(new
        {
            RegistryId = registryId,
            Id = id
        });
    }

    /// <summary>
    /// Gets the object by key id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="keyId">The key id</param>
    /// <returns></returns>
    public Task<RegistrySigningKeyModel> GetByKeyId(string registryId, string keyId)
    {
        return this.dataOps.Connect().QueryFirst("Registry.SigningKey", "GetByKeyId").ExecuteAsync<RegistrySigningKeyModel>(new
        {
            RegistryId = registryId,
            KeyId = keyId
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<RegistrySigningKeyModel> Create(RegistrySigningKeyCreateModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(RegistryObjects.REGISTRY_SIGNING_KEY)?.ToLowerInvariant();

        // perform operation
        return this.dataOps.Connect().QueryFirst("Registry.SigningKey", "Create").ExecuteAsync<RegistrySigningKeyModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="registryId">The registry id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<RegistrySigningKeyModel> DeleteById(string registryId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Registry.SigningKey", "DeleteById").ExecuteAsync<RegistrySigningKeyModel>(new
        {
            RegistryId = registryId,
            Id = id
        });    
    }
}