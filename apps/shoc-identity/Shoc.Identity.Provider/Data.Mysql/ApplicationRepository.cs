using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Application;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The application repository
/// </summary>
public class ApplicationRepository : IApplicationRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public ApplicationRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets the applications
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<ApplicationModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Identity.Application", "GetAll").ExecuteAsync<ApplicationModel>();
    }

    /// <summary>
    /// Gets the application by id
    /// </summary>
    /// <returns></returns>
    public Task<ApplicationModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Application", "GetById").ExecuteAsync<ApplicationModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Gets the application by id
    /// </summary>
    /// <returns></returns>
    public Task<ApplicationModel> GetByClientId(string applicationClientId)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Application", "GetByClientId").ExecuteAsync<ApplicationModel>(new
        {
            ApplicationClientId = applicationClientId
        });
    }

    /// <summary>
    /// Gets the client by application's client id
    /// </summary>
    /// <param name="applicationClientId">The client identifier</param>
    /// <returns></returns>
    public Task<ApplicationClientModel> GetClientByClientId(string applicationClientId)
    {
        return this.dataOps.Connect().MultiQuery("Identity.Application", "GetClientByClientId").ExecuteAsync(
            async multiResult =>
            {
                // try load application as a first set
                var application = await multiResult.ReadFirstOrDefaultAsync<ApplicationModel>();

                // consider no result if nothing is returned
                if (application == null)
                {
                    return null;
                }

                // map and return result
                return new ApplicationClientModel
                {
                    Application = application,
                    Secrets = await multiResult.ReadAsync<ApplicationSecretModel>(),
                    Uris = await multiResult.ReadAsync<ApplicationUriModel>(),
                    Claims = await multiResult.ReadAsync<ApplicationClaimModel>()
                };
            }, new
            {
                ApplicationClientId = applicationClientId
            }
        );
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<ApplicationModel> Create(ApplicationModel input)
    {
        // assign the id
        input.Id ??= StdIdGenerator.Next(IdentityObjects.APPLICATION)?.ToLowerInvariant();
        
        // create in the store
        return this.dataOps.Connect().QueryFirst("Identity.Application", "Create").ExecuteAsync<ApplicationModel>(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<ApplicationModel> UpdateById(string id, ApplicationModel input)
    {
        // make sure the id is properly set
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Identity.Application", "UpdateById").ExecuteAsync<ApplicationModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<ApplicationModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Identity.Application", "DeleteById").ExecuteAsync<ApplicationModel>(new
        {
            Id = id
        });
    }
}