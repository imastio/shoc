using System.Collections.Generic;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Shoc.Core;
using Shoc.Identity.Model;
using Shoc.Identity.Model.Application;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The application secret service
/// </summary>
public class ApplicationSecretService : ApplicationServiceBase
{
    /// <summary>
    /// The application secret repository
    /// </summary>
    private readonly IApplicationSecretRepository applicationSecretRepository;

    /// <summary>
    /// Creates new instance of application service
    /// </summary>
    /// <param name="applicationRepository">The application repository</param>
    /// <param name="applicationSecretRepository">The application secret repository</param>
    public ApplicationSecretService(IApplicationRepository applicationRepository, IApplicationSecretRepository applicationSecretRepository) : base(applicationRepository)
    {
        this.applicationSecretRepository = applicationSecretRepository;
    }
    
    /// <summary>
    /// Gets all the secrets for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    public async Task<IEnumerable<ApplicationSecretModel>> GetAll(string applicationId)
    {
        // make sure object exists
        await this.RequireById(applicationId);

        return await this.applicationSecretRepository.GetAll(applicationId);
    }

    /// <summary>
    /// Gets the secret for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the secret</param>
    /// <returns></returns>
    public async Task<ApplicationSecretModel> GetById(string applicationId, string id)
    {
        // make sure object exists
        await this.RequireById(applicationId);

        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try get object by id
        var result = await this.applicationSecretRepository.GetById(applicationId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }

    /// <summary>
    /// Creates a new secret
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    public async Task<ApplicationSecretModel> Create(string applicationId, ApplicationSecretModel input)
    {
        // make sure referring to the correct object
        input.ApplicationId = applicationId;

        // make sure object exists
        await this.RequireById(applicationId);

        // use shared secret as default
        input.Type ??= KnownSecretTypes.SHARED_SECRET;
        
        // hash secret in case of shared type
        if (input.Type == KnownSecretTypes.SHARED_SECRET)
        {
            input.Value = (input.Value ?? string.Empty).Sha512();
        }
        
        // perform create operation
        return await this.applicationSecretRepository.Create(input);
    }

    /// <summary>
    /// Updates the secret
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The secret id</param>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    public async Task<ApplicationSecretModel> UpdateById(string applicationId, string id, ApplicationSecretModel input)
    {
        // make sure referring to the correct object
        input.ApplicationId = applicationId;
        input.Id = id;
        
        // get the existing object and require to exist
        var existing = await this.GetById(applicationId, id);
        
        // use shared secret as default
        input.Type ??= KnownSecretTypes.SHARED_SECRET;
        
        // hash secret in case of shared type (if changed)
        if (input.Type == KnownSecretTypes.SHARED_SECRET && input.Value != existing.Value)
        {
            input.Value = (input.Value ?? string.Empty).ToSafeSha512();
        }

        return await this.applicationSecretRepository.UpdateById(input);
    }

    /// <summary>
    /// Deletes the secret for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the secret</param>
    /// <returns></returns>
    public async Task<ApplicationSecretModel> DeleteById(string applicationId, string id)
    {
        // make sure record exists
        await this.GetById(applicationId, id);

        // perform the operation
        var existing = await this.applicationSecretRepository.DeleteById(applicationId, id);

        // no such object 
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return existing object
        return existing;
    }
}