using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Identity.Model.Application;
using Shoc.Identity.Provider.Data;

namespace Shoc.Identity.Services;

/// <summary>
/// The application uri service
/// </summary>
public class ApplicationUriService : ApplicationServiceBase
{
    /// <summary>
    /// The application uri repository
    /// </summary>
    private readonly IApplicationUriRepository applicationUriRepository;

    /// <summary>
    /// Creates new instance of application service
    /// </summary>
    /// <param name="applicationRepository">The application repository</param>
    /// <param name="applicationUriRepository">The application uri repository</param>
    public ApplicationUriService(IApplicationRepository applicationRepository, IApplicationUriRepository applicationUriRepository) : base(applicationRepository)
    {
        this.applicationUriRepository = applicationUriRepository;
    }
    
    /// <summary>
    /// Gets all the uris for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <returns></returns>
    public async Task<IEnumerable<ApplicationUriModel>> GetAll(string applicationId)
    {
        // make sure object exists
        await this.RequireById(applicationId);

        return await this.applicationUriRepository.GetAll(applicationId);
    }

    /// <summary>
    /// Gets the uri for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the uri</param>
    /// <returns></returns>
    public async Task<ApplicationUriModel> GetById(string applicationId, string id)
    {
        // make sure object exists
        await this.RequireById(applicationId);

        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try get object by id
        var result = await this.applicationUriRepository.GetById(applicationId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }

    /// <summary>
    /// Creates a new uri
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="input">The input for creation</param>
    /// <returns></returns>
    public async Task<ApplicationUriModel> Create(string applicationId, ApplicationUriModel input)
    {
        // make sure referring to the correct object
        input.ApplicationId = applicationId;

        // make sure object exists
        await this.RequireById(applicationId);

        // use redirect uri as default
        input.Type ??= ApplicationUriTypes.REDIRECT_URI;

        // make sure input uri is given
        if (string.IsNullOrWhiteSpace(input.Uri))
        {
            throw ErrorDefinition.Validation().AsException();
        }
        
        // perform create operation
        return await this.applicationUriRepository.Create(input);
    }

    /// <summary>
    /// Updates the uri
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The uri id</param>
    /// <param name="input">The input for update</param>
    /// <returns></returns>
    public async Task<ApplicationUriModel> UpdateById(string applicationId, string id, ApplicationUriModel input)
    {
        // make sure referring to the correct object
        input.ApplicationId = applicationId;
        input.Id = id;
        
        // get the existing object and require to exist
        var existing = await this.GetById(applicationId, id);
        
        // use shared secret as default
        input.Type ??= ApplicationUriTypes.REDIRECT_URI;
        
        // make sure input uri is given
        if (string.IsNullOrWhiteSpace(input.Uri))
        {
            throw ErrorDefinition.Validation().AsException();
        }
        
        return await this.applicationUriRepository.UpdateById(input);
    }

    /// <summary>
    /// Deletes the uri for the application
    /// </summary>
    /// <param name="applicationId">The application id</param>
    /// <param name="id">The id of the uri</param>
    /// <returns></returns>
    public async Task<ApplicationUriModel> DeleteById(string applicationId, string id)
    {
        // make sure record exists
        await this.GetById(applicationId, id);

        // perform the operation
        var existing = await this.applicationUriRepository.DeleteById(applicationId, id);

        // no such object 
        if (existing == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return existing object
        return existing;
    }
}