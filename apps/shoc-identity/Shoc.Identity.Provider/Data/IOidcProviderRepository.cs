using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Provider;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The provider repository
/// </summary>
public interface IOidcProviderRepository
{
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<OidcProviderModel>> GetAll();
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<OidcProviderModel> GetById(string id);
    
    /// <summary>
    /// Gets the object by code
    /// </summary>
    /// <returns></returns>
    Task<OidcProviderModel> GetByCode(string code);

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<OidcProviderModel> Create(OidcProviderCreateModel input);

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<OidcProviderModel> UpdateById(string id, OidcProviderUpdateModel input);
    
    /// <summary>
    /// Updates the object secret by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<OidcProviderModel> UpdateClientSecretById(string id, OidcProviderClientSecretUpdateModel input);

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<OidcProviderModel> DeleteById(string id);
}
