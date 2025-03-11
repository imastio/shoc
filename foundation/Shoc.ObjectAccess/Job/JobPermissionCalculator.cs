using System.Collections.Generic;
using Shoc.ObjectAccess.Model.Job;
using Shoc.ObjectAccess.Model.Package;
using Shoc.ObjectAccess.Model.Workspace;

namespace Shoc.ObjectAccess.Job;

/// <summary>
/// Job permission calculator
/// </summary>
public class JobPermissionCalculator
{
    /// <summary>
    /// Gets the permissions assigned to the role
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="jobReference">The job reference</param>
    /// <param name="roles">The roles</param>
    /// <returns></returns>
    public virtual ISet<string> Calculate(string userId, JobAccessReferenceModel jobReference, IEnumerable<string> roles)
    {
        // resulting set of permissions
        var result = new HashSet<string>();

        // do for every role
        foreach (var role in roles)
        {
            // add role permissions
            result.UnionWith(Calculate(userId, jobReference, role));
        }
        
        // return resulting set
        return result;
    }

    /// <summary>
    /// Gets the permissions assigned to the role
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="jobReference">The job reference</param>
    /// <param name="role">The role to calculate</param>
    /// <returns></returns>
    private static ISet<string> Calculate(string userId, JobAccessReferenceModel jobReference, string role)
    {
        // resulting set of permissions
        var result = new HashSet<string>();

        // for workspace owners and admins we allow all operations
        if (role is WorkspaceRoles.ADMIN or WorkspaceRoles.OWNER)
        {
            return PackagePermissions.ALL;
        }

        // if job is user-scoped but the owner is not the requesting user then reject all permissions
        if (jobReference.Scope == JobScopes.USER && jobReference.UserId != userId)
        {
            return result;
        }
        
        // if package is workspace-scoped allowed to view and use by default
        result.UnionWith(new []{ JobPermissions.JOB_VIEW });

        // return resulting set
        return result;
    }
}