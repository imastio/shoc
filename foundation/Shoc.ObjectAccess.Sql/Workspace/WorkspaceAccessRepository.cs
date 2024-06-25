using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.ObjectAccess.Sql.Workspace;

/// <summary>
/// Workspace access repository
/// </summary>
public class WorkspaceAccessRepository : IWorkspaceAccessRepository
{
    /// <summary>
    /// The data ops reference
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of the repository
    /// </summary>
    /// <param name="dataOps">The data operations</param>
    public WorkspaceAccessRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets the role of the user in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace</param>
    /// <param name="userId">The user</param>
    /// <returns></returns>
    public Task<IEnumerable<WorkspaceUserRole>> GetRoles(string workspaceId, string userId)
    {
        return Guard.DoAsync(() => this.dataOps.Connect().Query("WorkspaceAccess", "GetRoles").ExecuteAsync<WorkspaceUserRole>(new
        {
            WorkspaceId = workspaceId,
            UserId = userId
        }));
    }
}