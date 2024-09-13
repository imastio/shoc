using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model.Application;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The application claim service
/// </summary>
public class ApplicationClaimService : ApplicationServiceBase
{
    /// <summary>
    /// The application uri repository
    /// </summary>
    private readonly IApplicationClaimRepository applicationClaimRepository;

    /// <summary>
    /// Creates new instance of application service
    /// </summary>
    /// <param name="applicationRepository">The application repository</param>
    /// <param name="applicationClaimRepository">The application claim repository</param>
    public ApplicationClaimService(IApplicationRepository applicationRepository, IApplicationClaimRepository applicationClaimRepository) : base(applicationRepository)
    {
        this.applicationClaimRepository = applicationClaimRepository;
    }
    
    /// <summary>
    /// Gets all the objects for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    public async Task<IEnumerable<ApplicationClaimModel>> GetAll(string applicationId)
    {
        // make sure object exists
        await this.RequireById(applicationId);

        return await this.applicationClaimRepository.GetAll(applicationId);
    }

    /// <summary>
    /// Gets the object for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<ApplicationClaimModel> GetById(string applicationId, string id)
    {
        // make sure object exists
        await this.RequireById(applicationId);

        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try get object by id
        var result = await this.applicationClaimRepository.GetById(applicationId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }

    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    public async Task<ApplicationClaimModel> Create(string applicationId, ApplicationClaimModel input)
    {
        // make sure referring to the correct object
        input.ApplicationId = applicationId;

        // make sure object exists
        await this.RequireById(applicationId);
        
        // make sure input type is given
        if (string.IsNullOrWhiteSpace(input.Type))
        {
            throw ErrorDefinition.Validation().AsException();
        }

        // empty value by default
        input.Value ??= string.Empty;
        
        // perform create operation
        return await this.applicationClaimRepository.Create(input);
    }

    /// <summary>
    /// Updates the object
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    public async Task<ApplicationClaimModel> UpdateById(string applicationId, string id, ApplicationClaimModel input)
    {
        // make sure referring to the correct object
        input.ApplicationId = applicationId;
        input.Id = id;
        
        // get the existing object and require to exist
        await this.GetById(applicationId, id);
        
        // make sure input type is given
        if (string.IsNullOrWhiteSpace(input.Type))
        {
            throw ErrorDefinition.Validation().AsException();
        }

        // empty value by default
        input.Value ??= string.Empty;
        
        return await this.applicationClaimRepository.UpdateById(input);
    }

    /// <summary>
    /// Deletes the object for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<ApplicationClaimModel> DeleteById(string applicationId, string id)
    {
        // make sure record exists
        await this.GetById(applicationId, id);

        // perform the operation
        var existing = await this.applicationClaimRepository.DeleteById(applicationId, id);

        // no such object 
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return existing object
        return existing;
    }
}