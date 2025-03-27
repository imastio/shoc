using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Provider;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The provider domain repository
/// </summary>
public class OidcProviderDomainRepository : IOidcProviderDomainRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public OidcProviderDomainRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the domains of the provider
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <returns></returns>
    public Task<IEnumerable<OidcProviderDomainModel>> GetAll(string providerId)
    {
        return this.dataOps.Connect().Query("Identity.OidcProvider.Domain", "GetAll").ExecuteAsync<OidcProviderDomainModel>(new
        {
           ProviderId = providerId 
        });
    }

    /// <summary>
    /// Gets the domain for the provider by id
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id of the uri</param>
    /// <returns></returns>
    public Task<OidcProviderDomainModel> GetById(string providerId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider.Domain", "GetById").ExecuteAsync<OidcProviderDomainModel>(new
        {
            ProviderId = providerId,
            Id = id
        });
    }

    /// <summary>
    /// Gets domain obejcts by domain name
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="domainName">The name of the domain</param>
    /// <returns></returns>
    public Task<OidcProviderDomainModel> GetByDomainName(string providerId, string domainName)
    {
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider.Domain", "GetByDomainName").ExecuteAsync<OidcProviderDomainModel>(new
        {
            ProviderId = providerId,
            DomainName = domainName
        });
    }

    /// <summary>
    /// Creates a new provider domain
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    public Task<OidcProviderDomainModel> Create(string providerId, OidcProviderDomainCreateModel input)
    {
        // ensure we refer to a parent object
        input.ProviderId = providerId;
        input.Id ??= StdIdGenerator.Next(IdentityObjects.OIDC_PROVIDER_DOMAIN)?.ToLowerInvariant();
        
        // create in the store
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider.Domain", "Create").ExecuteAsync<OidcProviderDomainModel>(input);
    }

    /// <summary>
    /// Updates the domain
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id</param>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    public Task<OidcProviderDomainModel> UpdateById(string providerId, string id, OidcProviderDomainUpdateModel input)
    {
        // ensure we refer to a correct object
        input.ProviderId = providerId;
        input.Id = id;
        
        // create in the store
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider.Domain", "UpdateById").ExecuteAsync<OidcProviderDomainModel>(input);
    }

    /// <summary>
    /// Deletes the domain for the provider
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<OidcProviderDomainModel> DeleteById(string providerId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.OidcProvider.Domain", "DeleteById").ExecuteAsync<OidcProviderDomainModel>(new
        {
            ProviderId = providerId,
            Id = id
        });
    }
}