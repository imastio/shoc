using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Provider;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The provider domain repository
/// </summary>
public interface IOidcProviderDomainRepository
{
    /// <summary>
    /// Gets all the domains of the provider
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <returns></returns>
    Task<IEnumerable<OidcProviderDomainModel>> GetAll(string providerId);

    /// <summary>
    /// Gets the domain for the provider by id
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id of the domain</param>
    /// <returns></returns>
    Task<OidcProviderDomainModel> GetById(string providerId, string id);
    
    /// <summary>
    /// Gets domain obejcts by domain name
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="domainName">The name of the domain</param>
    /// <returns></returns>
    Task<OidcProviderDomainModel> GetByDomainName(string providerId, string domainName);

    /// <summary>
    /// Creates a new provider domain
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    Task<OidcProviderDomainModel> Create(string providerId, OidcProviderDomainCreateModel input);

    /// <summary>
    /// Updates the domain
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id</param>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    Task<OidcProviderDomainModel> UpdateById(string providerId, string id, OidcProviderDomainUpdateModel input);
    
    /// <summary>
    /// Deletes the domain for the provider
    /// </summary>
    /// <param name="providerId">The provider id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<OidcProviderDomainModel> DeleteById(string providerId, string id);
}