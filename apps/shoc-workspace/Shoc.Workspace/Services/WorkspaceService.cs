using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Workspace.Data;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Services;

/// <summary>
/// The workspace service
/// </summary>
public class WorkspaceService : WorkspaceServiceBase
{
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="workspaceRepository">The object repository</param>
    /// <param name="workspaceUserRepository">The workspace user repository</param>
    public WorkspaceService(IWorkspaceRepository workspaceRepository, IWorkspaceUserRepository workspaceUserRepository) : base(workspaceRepository, workspaceUserRepository)
    {
    }

    /// <summary>
    /// Gets all objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<WorkspaceModel>> GetAll()
    {
        return this.workspaceRepository.GetAll();
    }
    
    /// <summary>
    /// Gets the workspace referential records
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<WorkspaceReferentialValueModel>> GetAllReferentialValues()
    {
        // load matching objects
        return this.workspaceRepository.GetAllReferentialValues();
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<WorkspaceModel> GetById(string id)
    {
        return this.RequireWorkspaceById(id);
    }
        
    /// <summary>
    /// Creates new object
    /// </summary>
    /// <param name="input">The object creation model</param>
    /// <returns></returns>
    public async Task<WorkspaceModel> Create(WorkspaceCreateModel input)
    {
        // validate type
        ValidateType(input.Type);
        
        // validate name
        ValidateName(input.Name);
        
        // initialize the status
        input.Status = WorkspaceStatuses.ACTIVE;

        // make sure title exists otherwise use name
        input.Title = string.IsNullOrWhiteSpace(input.Title) ? input.Name : input.Title;

        // try getting by name
        var existing = await this.workspaceRepository.GetByName(input.Name);

        // already exists
        if (existing != null)
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.EXISTING_NAME).AsException();
        }

        // require user to be there
        await this.RequireUser(input.CreatedBy);
        
        // perform the operation
        return await this.workspaceRepository.Create(input);
    }

    /// <summary>
    /// Updates the object 
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The object update model</param>
    /// <returns></returns>
    public async Task<WorkspaceModel> UpdateById(string id, WorkspaceUpdateModel input)
    {
        // make sure referring the proper object
        input.Id = id;

        // make sure object exists 
        var existing = await this.GetById(id);
        
        // validate the name
        ValidateName(input.Name);
        
        // validate status
        ValidateStatus(input.Status);
        
        // make sure title exists otherwise use name
        input.Title = string.IsNullOrWhiteSpace(input.Title) ? input.Name : input.Title;

        // try getting by name
        var byName = await this.workspaceRepository.GetByName(input.Name);

        // if exists object by the name and is not the same object then throw error
        if (byName != null && existing.Id != byName.Id)
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.EXISTING_NAME).AsException();
        }
        
        // perform the operation
        return await this.workspaceRepository.UpdateById(id, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    public async Task<WorkspaceModel> DeleteById(string id)
    {
        // try deleting object by id
        var result = await this.workspaceRepository.DeleteById(id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }
}