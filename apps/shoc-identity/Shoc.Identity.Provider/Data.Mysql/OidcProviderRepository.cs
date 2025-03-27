using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Provider;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The repository implementation
/// </summary>
public class OidcProviderRepository : IOidcProviderRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public OidcProviderRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<OidcProviderModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.OidcProvider", "GetAll").ExecuteAsync<OidcProviderModel>();
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<OidcProviderModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider", "GetById").ExecuteAsync<OidcProviderModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Gets the object by code
    /// </summary>
    /// <returns></returns>
    public Task<OidcProviderModel> GetByCode(string code)
    {
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider", "GetByCode").ExecuteAsync<OidcProviderModel>(new
        {
            Code = code
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<OidcProviderModel> Create(OidcProviderCreateModel input)
    {
        // assign the id
        input.Id ??= StdIdGenerator.Next(IdentityObjects.OIDC_PROVIDER)?.ToLowerInvariant();
        
        // create in the store
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider", "Create").ExecuteAsync<OidcProviderModel>(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<OidcProviderModel> UpdateById(string id, OidcProviderUpdateModel input)
    {
        // ensure referring to the correct object
        input.Id = id;
        
        // create in the store
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider", "UpdateById").ExecuteAsync<OidcProviderModel>(input);
    }

    /// <summary>
    /// Updates the object secret by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<OidcProviderModel> UpdateClientSecretById(string id, OidcProviderClientSecretUpdateModel input)
    {
        // ensure referring to the correct object
        input.Id = id;
        
        // create in the store
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider", "UpdateClientSecretById").ExecuteAsync<OidcProviderModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<OidcProviderModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider", "DeleteById").ExecuteAsync<OidcProviderModel>(new
        {
            Id = id
        });
    }
}