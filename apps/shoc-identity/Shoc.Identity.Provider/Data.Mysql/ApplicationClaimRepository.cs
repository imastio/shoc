using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Application;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The application claim repository
/// </summary>
public class ApplicationClaimRepository : IApplicationClaimRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public ApplicationClaimRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    public Task<IEnumerable<ApplicationClaimModel>> GetAll(string applicationId)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().Query("Identity.Application.Claim", "GetAll").ExecuteAsync<ApplicationClaimModel>(new
        {
            ApplicationId = applicationId
        }));
    }

    /// <summary>
    /// Gets the object for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<ApplicationClaimModel> GetById(string applicationId, string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Claim", "GetById").ExecuteAsync<ApplicationClaimModel>(new
        {
            ApplicationId = applicationId,
            Id = id
        }));
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    public Task<ApplicationClaimModel> Create(ApplicationClaimModel input)
    {
        // assign an identifier
        input.Id = StdIdGenerator.Next(IdentityObjects.APPLICATION_CLAIM);

        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Claim", "Create").ExecuteAsync<ApplicationClaimModel>(input));
    }

    /// <summary>
    /// Updates the object
    /// </summary>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    public Task<ApplicationClaimModel> UpdateById(ApplicationClaimModel input)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Claim", "UpdateById").ExecuteAsync<ApplicationClaimModel>(input));
    }
    
    /// <summary>
    /// Deletes the object for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<ApplicationClaimModel> DeleteById(string applicationId, string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Claim", "DeleteById").ExecuteAsync<ApplicationClaimModel>(new
        {
            ApplicationId = applicationId,
            Id = id
        }));
    }
}