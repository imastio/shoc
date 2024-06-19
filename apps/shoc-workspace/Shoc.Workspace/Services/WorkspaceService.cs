using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.Workspace.Data;
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
    public WorkspaceService(IWorkspaceRepository workspaceRepository) : base(workspaceRepository)
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
    /// Gets the object by name
    /// </summary>
    /// <param name="name">The name of the object</param>
    /// <returns></returns>
    public async Task<WorkspaceModel> GetByName(string name)
    {
        // name should be a valid string
        if (string.IsNullOrWhiteSpace(name))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by name
        var result = await this.workspaceRepository.GetByName(name);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
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
        
        // initialize the status
        input.Status = WorkspaceStatuses.ACTIVE;

        // TODO: Implement
        throw ErrorDefinition.Data().AsException();
        
        // perform the operation
        return await this.workspaceRepository.Create(input);
    }

    /// <summary>
    /// Updates the employee 
    /// </summary>
    /// <param name="id">The employee id</param>
    /// <param name="input">The employee update model</param>
    /// <returns></returns>
    public async Task<WorkspaceModel> UpdateById(string id, WorkspaceUpdateModel input)
    {
        // make sure referring the proper object
        input.Id = id;

        // make sure object exists 
        await this.GetById(id);

        // TODO: Implement
        throw ErrorDefinition.Data().AsException();

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