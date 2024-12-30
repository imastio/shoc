using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ObjectAccess.Model;

namespace Shoc.ObjectAccess.Package;

/// <summary>
/// Evaluate package access
/// </summary>
public interface IPackageAccessEvaluator
{
    /// <summary>
    /// Gets the permissions of the given user to the given package in the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="packageId">The package id</param>
    /// <returns></returns>
    Task<ISet<string>> GetPermissions(string userId, string workspaceId, string packageId);

    /// <summary>
    /// Evaluate workspace package access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="packageId">The package id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    Task<AccessEvaluationResult> Evaluate(string userId, string workspaceId, string packageId, params string[] permissions);

    /// <summary>
    /// Evaluate workspace package access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="packageId">The package id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    Task Ensure(string userId, string workspaceId, string packageId, params string[] permissions);
}