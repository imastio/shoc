using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;
using Shoc.Workspace.Data;
using Shoc.Workspace.Model.UserWorkspace;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Services;

/// <summary>
/// The user workspace service
/// </summary>
public class UserWorkspaceService : UserWorkspaceServiceBase
{
    /// <summary>
    /// The base implementation of the service
    /// </summary>
    /// <param name="workspaceService">The workspace service</param>
    /// <param name="userWorkspaceRepository">The user workspace repository</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    public UserWorkspaceService(WorkspaceService workspaceService, IUserWorkspaceRepository userWorkspaceRepository, IWorkspaceAccessEvaluator workspaceAccessEvaluator) : base(workspaceService, userWorkspaceRepository, workspaceAccessEvaluator)
    {
    }

    /// <summary>
    /// Gets all objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<UserWorkspaceModel>> GetAll(string userId)
    {
        return this.userWorkspaceRepository.GetAll(userId);
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<UserWorkspaceModel> GetById(string userId, string id)
    {
        // get the result
        var result = await this.RequireById(userId, id);

        // ensure the access
        await this.workspaceAccessEvaluator.Ensure(userId, id, WorkspacePermissions.WORKSPACE_VIEW);

        return result;
    }

    /// <summary>
    /// Gets object permissions by id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    public async Task<ISet<string>> GetPermissionsById(string userId, string id)
    {
        // make sure object exists
        await this.RequireById(userId, id);
        
        return await this.workspaceAccessEvaluator.GetPermissions(userId, id);
    }
    
    /// <summary>
    /// Gets object permissions by name
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="name">The name of object</param>
    /// <returns></returns>
    public async Task<ISet<string>> GetPermissionsByName(string userId, string name)
    {
        // make sure object exists
        var result = await this.RequireByName(userId, name);
        
        return await this.workspaceAccessEvaluator.GetPermissions(userId, result.Id);
    }
    
    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="name">The name of the object</param>
    /// <returns></returns>
    public async Task<UserWorkspaceModel> GetByName(string userId, string name)
    {
        // get the result
        var result = await this.RequireByName(userId, name);

        // ensure the access
        await this.workspaceAccessEvaluator.Ensure(userId, result.Id, WorkspacePermissions.WORKSPACE_VIEW);

        return result;
    }

    /// <summary>
    /// Creates new object
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="input">The object creation model</param>
    /// <returns></returns>
    public async Task<UserWorkspaceCreatedModel> Create(string userId, UserWorkspaceCreateModel input)
    {
        // perform the operation
        var result =  await this.workspaceService.Create(new WorkspaceCreateModel
        {
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Status = WorkspaceStatuses.ACTIVE,
            CreatedBy = userId
        });

        // return result
        return new UserWorkspaceCreatedModel
        {
            Id = result.Id,
            UserId = userId
        };
    }

    /// <summary>
    /// Updates the object 
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The object update model</param>
    /// <returns></returns>
    public async Task<UserWorkspaceUpdatedModel> UpdateById(string userId, string id, UserWorkspaceUpdateModel input)
    {
        // make sure referring the proper object
        input.Id = id;

        // make sure object exists 
        var existing = await this.RequireById(userId, id);
        
        // ensure update permissions is given
        await this.workspaceAccessEvaluator.Ensure(userId, id, WorkspacePermissions.WORKSPACE_UPDATE);

        // perform the operation
        var result = await this.workspaceService.UpdateById(id, new WorkspaceUpdateModel
        {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Status = existing.Status
        });
        
        // return the result
        return new UserWorkspaceUpdatedModel
        {
            Id = result.Id,
            UserId = userId
        };
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="id">The id of object</param>
    /// <returns></returns>
    public async Task<UserWorkspaceDeletedModel> DeleteById(string userId, string id)
    {
        // require the object
        await this.RequireById(userId, id);

        // ensure permissions
        await this.workspaceAccessEvaluator.Ensure(userId, id, WorkspacePermissions.WORKSPACE_DELETE);
        
        // perform the operation
        var result = await this.workspaceService.DeleteById(id);
        
        // return result
        return new UserWorkspaceDeletedModel
        {
            Id = result.Id,
            UserId = userId
        };
    }
}