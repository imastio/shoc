using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Job.Data;
using Shoc.Job.Model.GitRepo;

namespace Shoc.Job.Services;

/// <summary>
/// The git repo service
/// </summary>
public class GitRepoService
{
    /// <summary>
    /// The git repo repository
    /// </summary>
    private readonly IGitRepoRepository gitRepoRepository;

    /// <summary>
    /// The git repo validation service
    /// </summary>
    private readonly GitRepoValidationService validationService;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="gitRepoRepository">The git repo repository</param>
    /// <param name="validationService">The validation service</param>
    public GitRepoService(IGitRepoRepository gitRepoRepository, GitRepoValidationService validationService)
    {
        this.gitRepoRepository = gitRepoRepository;
        this.validationService = validationService;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<GitRepoModel>> GetAll(string workspaceId)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        return await this.gitRepoRepository.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public async Task<GitRepoModel> GetById(string workspaceId, string id)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        return await this.gitRepoRepository.GetById(workspaceId, id);
    }
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<GitRepoModel> Create(string workspaceId, GitRepoCreateModel input)
    {
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // validate the input
        await this.ValidateCreationInput(workspaceId, input);
        
        // try getting object by source and repository
        var existing = await this.gitRepoRepository.GetBySourceAndRepository(workspaceId, input.Source, input.Repository);

        // if object exists
        if (existing != null)
        {
            throw ErrorDefinition.Validation().AsException();
        }
        
        // create object in the storage
        return await this.gitRepoRepository.Create(workspaceId, input);
    }
    
    /// <summary>
    /// Ensures the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<GitRepoModel> Ensure(string workspaceId, GitRepoCreateModel input)
    {
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // validate the input
        await this.ValidateCreationInput(workspaceId, input);
        
        // try getting object by source and repository
        var existing = await this.gitRepoRepository.GetBySourceAndRepository(workspaceId, input.Source, input.Repository);

        // if object exists
        if (existing != null)
        {
            return existing;
        }
        
        // create object in the storage
        return await this.gitRepoRepository.Ensure(workspaceId, input);
    }
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<GitRepoModel> DeleteById(string workspaceId, string id)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);
        
        // try deleting object by id
        var result = await this.gitRepoRepository.DeleteById(workspaceId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }

    /// <summary>
    /// Validates the creation input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The input to validate</param>
    private async Task ValidateCreationInput(string workspaceId, GitRepoCreateModel input)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);
        
        // validate name
        this.validationService.ValidateName(input.Name);
        
        // validate owner
        this.validationService.ValidateOwner(input.Owner);
        
        // validate source
        this.validationService.ValidateSource(input.Source);

        // validate repository
        this.validationService.ValidateSource(input.Repository);
        
        // validate remote url
        this.validationService.ValidateRemoteUrl(input.RemoteUrl);
    }
}