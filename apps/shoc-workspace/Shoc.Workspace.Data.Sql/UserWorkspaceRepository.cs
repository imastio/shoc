using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Workspace.Model.UserWorkspace;

namespace Shoc.Workspace.Data.Sql;

/// <summary>
/// The user workspace repository
/// </summary>
public class UserWorkspaceRepository : IUserWorkspaceRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public UserWorkspaceRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<UserWorkspaceModel>> GetAll(string userId)
    {
        return this.dataOps.Connect().Query("Workspace.UserWorkspace", "GetAll").ExecuteAsync<UserWorkspaceModel>(new
        {
           UserId = userId 
        });
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    public Task<UserWorkspaceModel> GetById(string userId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.UserWorkspace", "GetById").ExecuteAsync<UserWorkspaceModel>(new
        {
            UserId = userId,
            Id = id
        });
    }

    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <returns></returns>
    public Task<UserWorkspaceModel> GetByName(string userId, string name)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.UserWorkspace", "GetByName").ExecuteAsync<UserWorkspaceModel>(new
        {
            UserId = userId,
            Name = name
        });
    }
}