using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Data.Sql;

/// <summary>
/// The repository implementation
/// </summary>
public class WorkspaceRepository : IWorkspaceRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public WorkspaceRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<WorkspaceModel>> GetAll()
    {
        return this.dataOps.Connect().Query("Workspace.Workspace", "GetAll").ExecuteAsync<WorkspaceModel>();
    }

    /// <summary>
    /// Gets the application by id
    /// </summary>
    /// <returns></returns>
    public Task<WorkspaceModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace", "GetById").ExecuteAsync<WorkspaceModel>(new
        {
            Id = id
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<WorkspaceModel> Create(WorkspaceCreateModel input)
    {
        // prepare argument list
        var argument = new DynamicParameters();
        
        // assign the id
        input.Id ??= StdIdGenerator.Next(WorkspaceObjects.WORKSPACE).ToLowerInvariant();
        
        // add input to argument
        argument.AddDynamicParams(input);
        
        // add owner membership record
        argument.AddDynamicParams(new
        {
            OwnerMembershipId = StdIdGenerator.Next(WorkspaceObjects.WORKSPACE_MEMBER).ToLowerInvariant(),
            UserId = input.CreatedBy,
            Role = WorkspaceRoles.OWNER
        });
        
        // create in the store
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace", "Create").ExecuteAsync<WorkspaceModel>(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<WorkspaceModel> UpdateById(string id, WorkspaceUpdateModel input)
    {
        // make sure the id is properly set
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace", "UpdateById").ExecuteAsync<WorkspaceModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<WorkspaceModel> DeleteById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace", "DeleteById").ExecuteAsync<WorkspaceModel>(new
        {
            Id = id
        });
    }
}