using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Application;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The application uri repository
/// </summary>
public interface IApplicationUriRepository
{
    /// <summary>
    /// Gets all the uris for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    Task<IEnumerable<ApplicationUriModel>> GetAll(string applicationId);

    /// <summary>
    /// Gets the uri for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the uri</param>
    /// <returns></returns>
    Task<ApplicationUriModel> GetById(string applicationId, string id);

    /// <summary>
    /// Creates a new uri
    /// </summary>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    Task<ApplicationUriModel> Create(ApplicationUriModel input);

    /// <summary>
    /// Updates the uri
    /// </summary>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    Task<ApplicationUriModel> UpdateById(ApplicationUriModel input);
    
    /// <summary>
    /// Deletes the uri for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the uri</param>
    /// <returns></returns>
    Task<ApplicationUriModel> DeleteById(string applicationId, string id);
}