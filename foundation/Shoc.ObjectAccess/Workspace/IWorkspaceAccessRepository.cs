using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ObjectAccess.Model.Workspace;

namespace Shoc.ObjectAccess.Workspace;

/// <summary>
/// Workspace access repository
/// </summary>
public interface IWorkspaceAccessRepository
{
    /// <summary>
    /// Gets the role of the user in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace</param>
    /// <param name="userId">The user</param>
    /// <returns></returns>
    Task<IEnumerable<WorkspaceUserRole>> GetRoles(string workspaceId, string userId);
}