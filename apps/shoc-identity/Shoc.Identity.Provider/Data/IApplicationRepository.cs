using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Application;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The client repository
/// </summary>
public interface IApplicationRepository
{
    /// <summary>
    /// Gets the applications
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ApplicationModel>> GetAll();
    
    /// <summary>
    /// Gets the application by id
    /// </summary>
    /// <returns></returns>
    Task<ApplicationModel> GetById(string id);
    
    /// <summary>
    /// Gets the application by id
    /// </summary>
    /// <returns></returns>
    Task<ApplicationModel> GetByClientId(string applicationClientId);
    
    /// <summary>
    /// Gets the client by application's client id
    /// </summary>
    /// <param name="applicationClientId">The client identifier</param>
    /// <returns></returns>
    Task<ApplicationClientModel> GetClientByClientId(string applicationClientId);

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    Task<ApplicationModel> Create(ApplicationModel input);

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    Task<ApplicationModel> UpdateById(string id, ApplicationModel input);

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<ApplicationModel> DeleteById(string id);
}
