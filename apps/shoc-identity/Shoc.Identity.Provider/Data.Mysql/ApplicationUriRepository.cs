using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Application;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The application uri repository
/// </summary>
public class ApplicationUriRepository : IApplicationUriRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public ApplicationUriRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the uris for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    public Task<IEnumerable<ApplicationUriModel>> GetAll(string applicationId)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().Query("Identity.Application.Uri", "GetAll").ExecuteAsync<ApplicationUriModel>(new
        {
            ApplicationId = applicationId
        }));
    }

    /// <summary>
    /// Gets the uri for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the uri</param>
    /// <returns></returns>
    public Task<ApplicationUriModel> GetById(string applicationId, string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Uri", "GetById").ExecuteAsync<ApplicationUriModel>(new
        {
            ApplicationId = applicationId,
            Id = id
        }));
    }

    /// <summary>
    /// Creates a new uri
    /// </summary>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    public Task<ApplicationUriModel> Create(ApplicationUriModel input)
    {
        // assign an identifier
        input.Id = StdIdGenerator.Next(IdentityObjects.APPLICATION_URI);

        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Uri", "Create").ExecuteAsync<ApplicationUriModel>(input));
    }

    /// <summary>
    /// Updates the uri
    /// </summary>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    public Task<ApplicationUriModel> UpdateById(ApplicationUriModel input)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Uri", "UpdateById").ExecuteAsync<ApplicationUriModel>(input));
    }
    
    /// <summary>
    /// Deletes the uri for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the uri</param>
    /// <returns></returns>
    public Task<ApplicationUriModel> DeleteById(string applicationId, string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Uri", "DeleteById").ExecuteAsync<ApplicationUriModel>(new
        {
            ApplicationId = applicationId,
            Id = id
        }));
    }
}