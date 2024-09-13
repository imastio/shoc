using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ObjectAccess.Model;

namespace Shoc.ObjectAccess.Workspace;

/// <summary>
/// Evaluate workspace access
/// </summary>
public interface IWorkspaceAccessEvaluator
{
    /// <summary>
    /// Gets the permissions of the given user to the given workspac
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    Task<ISet<string>> GetPermissions(string userId, string workspaceId);
    
    /// <summary>
    /// Evaluate workspace access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    Task<AccessEvaluationResult> Evaluate(string userId, string workspaceId, params string[] permissions);
    
    /// <summary>
    /// Evaluate workspace access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    Task Ensure(string userId, string workspaceId, params string[] permissions);
}