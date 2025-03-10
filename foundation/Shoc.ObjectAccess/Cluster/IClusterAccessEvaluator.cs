using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ObjectAccess.Model;

namespace Shoc.ObjectAccess.Cluster;

/// <summary>
/// Evaluate cluster access
/// </summary>
public interface IClusterAccessEvaluator
{
    /// <summary>
    /// Gets the permissions of the given user to the given cluster in the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="clusterId">The cluster id</param>
    /// <returns></returns>
    Task<ISet<string>> GetPermissions(string userId, string workspaceId, string clusterId);

    /// <summary>
    /// Evaluate workspace cluster access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="clusterId">The cluster id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    Task<AccessEvaluationResult> Evaluate(string userId, string workspaceId, string clusterId, params string[] permissions);

    /// <summary>
    /// Evaluate workspace cluster access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="clusterId">The cluster id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    Task Ensure(string userId, string workspaceId, string clusterId, params string[] permissions);
}