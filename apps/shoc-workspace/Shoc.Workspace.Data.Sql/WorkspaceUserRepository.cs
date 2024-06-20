using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Workspace.Model.Common;

namespace Shoc.Workspace.Data.Sql;

/// <summary>
/// The repository implementation
/// </summary>
public class WorkspaceUserRepository : IWorkspaceUserRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public WorkspaceUserRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets the application by id
    /// </summary>
    /// <returns></returns>
    public Task<WorkspaceUserModel> GetById(string id)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.User", "GetById").ExecuteAsync<WorkspaceUserModel>(new
        {
            Id = id
        });
    }
}