using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Identity.Model.Application;

namespace Shoc.Identity.Provider.Data;

/// <summary>
/// The application claim repository
/// </summary>
public interface IApplicationClaimRepository
{
    /// <summary>
    /// Gets all the objects for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    Task<IEnumerable<ApplicationClaimModel>> GetAll(string applicationId);

    /// <summary>
    /// Gets the claim for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<ApplicationClaimModel> GetById(string applicationId, string id);

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    Task<ApplicationClaimModel> Create(ApplicationClaimModel input);

    /// <summary>
    /// Updates the object
    /// </summary>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    Task<ApplicationClaimModel> UpdateById(ApplicationClaimModel input);
    
    /// <summary>
    /// Deletes the object for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    Task<ApplicationClaimModel> DeleteById(string applicationId, string id);
}