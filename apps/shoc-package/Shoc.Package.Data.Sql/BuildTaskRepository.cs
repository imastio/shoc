using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Package.Model;
using Shoc.Package.Model.BuildTask;

namespace Shoc.Package.Data.Sql;

/// <summary>
/// The repository interface implementation
/// </summary>
public class BuildTaskRepository : IBuildTaskRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public BuildTaskRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<BuildTaskModel>> GetAll(string workspaceId)
    {
        return this.dataOps.Connect().Query("Package.BuildTask", "GetAll").ExecuteAsync<BuildTaskModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<BuildTaskModel> GetById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Package.BuildTask", "GetById").ExecuteAsync<BuildTaskModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public Task<BuildTaskModel> Create(string workspaceId, BuildTaskCreateModel input)
    {
        // assign the id
        input.WorkspaceId = workspaceId;
        input.Id ??= StdIdGenerator.Next(PackageObjects.BUILD_TASK).ToLowerInvariant();

        return this.dataOps.Connect().QueryFirst("Package.BuildTask", "Create").ExecuteAsync<BuildTaskModel>(input);
    }

    /// <summary>
    /// Updates the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The object id</param>
    /// <param name="input">The update input</param>
    /// <returns></returns>
    public Task<BuildTaskModel> UpdateById(string workspaceId, string id, BuildTaskUpdateModel input)
    {
        input.WorkspaceId = workspaceId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Package.BuildTask", "UpdateById").ExecuteAsync<BuildTaskModel>(input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public Task<BuildTaskModel> DeleteById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Package.BuildTask", "DeleteById").ExecuteAsync<BuildTaskModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
}