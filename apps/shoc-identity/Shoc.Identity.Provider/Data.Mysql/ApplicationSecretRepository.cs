using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Application;

namespace Shoc.Identity.Provider.Data.Mysql;

/// <summary>
/// The application secret repository
/// </summary>
public class ApplicationSecretRepository : IApplicationSecretRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public ApplicationSecretRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the secrets for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    public Task<IEnumerable<ApplicationSecretModel>> GetAll(string applicationId)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().Query("Identity.Application.Secret", "GetAll").ExecuteAsync<ApplicationSecretModel>(new
        {
            ApplicationId = applicationId
        }));
    }

    /// <summary>
    /// Gets the secret for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the secret</param>
    /// <returns></returns>
    public Task<ApplicationSecretModel> GetById(string applicationId, string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Secret", "GetById").ExecuteAsync<ApplicationSecretModel>(new
        {
            ApplicationId = applicationId,
            Id = id
        }));
    }

    /// <summary>
    /// Creates a new secret
    /// </summary>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    public Task<ApplicationSecretModel> Create(ApplicationSecretModel input)
    {
        // assign an identifier
        input.Id = StdIdGenerator.Next(IdentityObjects.APPLICATION_SECRET);

        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Secret", "Create").ExecuteAsync<ApplicationSecretModel>(input));
    }

    /// <summary>
    /// Updates the secret
    /// </summary>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    public Task<ApplicationSecretModel> UpdateById(ApplicationSecretModel input)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Secret", "UpdateById").ExecuteAsync<ApplicationSecretModel>(input));
    }
    
    /// <summary>
    /// Deletes the secret for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the secret</param>
    /// <returns></returns>
    public Task<ApplicationSecretModel> DeleteById(string applicationId, string id)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().QueryFirst("Identity.Application.Secret", "DeleteById").ExecuteAsync<ApplicationSecretModel>(new
        {
            ApplicationId = applicationId,
            Id = id
        }));
    }
}