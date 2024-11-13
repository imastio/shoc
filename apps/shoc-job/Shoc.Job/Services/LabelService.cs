using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Job.Data;
using Shoc.Job.Model;
using Shoc.Job.Model.Label;

namespace Shoc.Job.Services;

/// <summary>
/// The labels service
/// </summary>
public class LabelService
{
    /// <summary>
    /// The label repository
    /// </summary>
    private readonly ILabelRepository labelRepository;

    /// <summary>
    /// The label validation service
    /// </summary>
    private readonly LabelValidationService validationService;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="labelRepository">The label repository</param>
    /// <param name="validationService">The label validation service</param>
    public LabelService(ILabelRepository labelRepository, LabelValidationService validationService)
    {
        this.labelRepository = labelRepository;
        this.validationService = validationService;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<LabelModel>> GetAll(string workspaceId)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        return await this.labelRepository.GetAll(workspaceId);
    }
    
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public async Task<LabelModel> GetById(string workspaceId, string id)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);

        // get from the storage
        return await this.labelRepository.GetById(workspaceId, id);
    }
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<LabelModel> Create(string workspaceId, LabelCreateModel input)
    {
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);
        
        // validate name
        this.validationService.ValidateName(input.Name);

        // try getting object by name
        var existing = await this.labelRepository.GetByName(workspaceId, input.Name);

        // if object by name exists
        if (existing != null)
        {
            throw ErrorDefinition.Validation(JobErrors.EXISTING_LABEL_NAME).AsException();
        }
        
        // create object in the storage
        return await this.labelRepository.Create(workspaceId, input);
    }
    
    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<LabelModel> DeleteById(string workspaceId, string id)
    {
        // require the parent object
        await this.validationService.RequireWorkspace(workspaceId);
        
        // try deleting object by id
        var result = await this.labelRepository.DeleteById(workspaceId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }
}