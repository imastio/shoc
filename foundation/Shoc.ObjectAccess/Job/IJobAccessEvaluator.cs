using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ObjectAccess.Model;

namespace Shoc.ObjectAccess.Job;

/// <summary>
/// Evaluate job access
/// </summary>
public interface IJobAccessEvaluator
{
    /// <summary>
    /// Gets the permissions of the given user to the given job in the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <returns></returns>
    Task<ISet<string>> GetPermissions(string userId, string workspaceId, string jobId);

    /// <summary>
    /// Evaluate workspace job access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    Task<AccessEvaluationResult> Evaluate(string userId, string workspaceId, string jobId, params string[] permissions);

    /// <summary>
    /// Evaluate workspace job access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    Task Ensure(string userId, string workspaceId, string jobId, params string[] permissions);
}