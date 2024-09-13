using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Application;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The application secret repository
/// </summary>
public interface IApplicationSecretRepository
{
    /// <summary>
    /// Gets all the secrets for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    Task<IEnumerable<ApplicationSecretModel>> GetAll(string applicationId);

    /// <summary>
    /// Gets the secret for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the secret</param>
    /// <returns></returns>
    Task<ApplicationSecretModel> GetById(string applicationId, string id);

    /// <summary>
    /// Creates a new secret
    /// </summary>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    Task<ApplicationSecretModel> Create(ApplicationSecretModel input);

    /// <summary>
    /// Updates the secret
    /// </summary>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    Task<ApplicationSecretModel> UpdateById(ApplicationSecretModel input);
    
    /// <summary>
    /// Deletes the secret for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the secret</param>
    /// <returns></returns>
    Task<ApplicationSecretModel> DeleteById(string applicationId, string id);
}