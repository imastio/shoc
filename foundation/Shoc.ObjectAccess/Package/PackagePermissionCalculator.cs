using System.Collections.Generic;
using Shoc.ObjectAccess.Model.Package;
using Shoc.ObjectAccess.Model.Workspace;

namespace Shoc.ObjectAccess.Package;

/// <summary>
/// Package permission calculator
/// </summary>
public class PackagePermissionCalculator
{
    /// <summary>
    /// Gets the permissions assigned to the role
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="packageReference">The package reference</param>
    /// <param name="roles">The roles</param>
    /// <returns></returns>
    public virtual ISet<string> Calculate(string userId, PackageAccessReferenceModel packageReference, IEnumerable<string> roles)
    {
        // resulting set of permissions
        var result = new HashSet<string>();

        // do for every role
        foreach (var role in roles)
        {
            // add role permissions
            result.UnionWith(Calculate(userId, packageReference, role));
        }
        
        // return resulting set
        return result;
    }

    /// <summary>
    /// Gets the permissions assigned to the role
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="packageReference">The package reference</param>
    /// <param name="role">The role to calculate</param>
    /// <returns></returns>
    private static ISet<string> Calculate(string userId, PackageAccessReferenceModel packageReference, string role)
    {
        // resulting set of permissions
        var result = new HashSet<string>();

        // for workspace owners and admins we allow all operations
        if (role is WorkspaceRoles.ADMIN or WorkspaceRoles.OWNER)
        {
            return PackagePermissions.ALL;
        }

        // if package is user-scoped but the owner is not the requesting user then reject all permissions
        if (packageReference.Scope == PackageScopes.USER && packageReference.UserId != userId)
        {
            return result;
        }
        
        // if package is workspace-scoped allowed to view and use by default
        result.UnionWith(new []{ PackagePermissions.PACKAGE_VIEW, PackagePermissions.PACKAGE_USE });

        // return resulting set
        return result;
    }
}