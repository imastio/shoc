using System.Threading.Tasks;
using Shoc.Workspace.Model.Common;

namespace Shoc.Workspace.Data;

/// <summary>
/// The workspace user repository
/// </summary>
public interface IWorkspaceUserRepository
{
    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <returns></returns>
    Task<WorkspaceUserModel> GetById(string id);
}