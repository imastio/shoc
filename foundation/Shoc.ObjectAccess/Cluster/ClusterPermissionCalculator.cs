using System.Collections.Generic;
using Shoc.ObjectAccess.Model.Cluster;
using Shoc.ObjectAccess.Model.Workspace;

namespace Shoc.ObjectAccess.Cluster;

/// <summary>
/// Cluster permission calculator
/// </summary>
public class ClusterPermissionCalculator
{
    /// <summary>
    /// The static mappings between workspace roles and permissions
    /// </summary>
    private static readonly IDictionary<string, ISet<string>> ROLE_PERMISSIONS = new Dictionary<string, ISet<string>>
    {
        {
            WorkspaceRoles.OWNER,
            ClusterPermissions.ALL
        },
        {
            WorkspaceRoles.ADMIN,
            ClusterPermissions.ALL
        },
        {
            WorkspaceRoles.MEMBER,
            new HashSet<string>
            {
                ClusterPermissions.CLUSTER_VIEW,
                ClusterPermissions.CLUSTER_USE
            }
        },
        {
            WorkspaceRoles.GUEST,
            new HashSet<string>
            {
                ClusterPermissions.CLUSTER_VIEW,
            }
        }
    };
        
    /// <summary>
    /// Gets the permissions assigned to the role
    /// </summary>
    /// <param name="roles">The roles</param>
    /// <returns></returns>
    public virtual ISet<string> Calculate(params string[] roles)
    {
        // resulting set of permissions
        var result = new HashSet<string>();

        // do for every role
        foreach (var role in roles)
        {
            // no such role
            if (!ROLE_PERMISSIONS.TryGetValue(role, out var assigned))
            {
                continue;
            }
            
            // add role permissions
            result.UnionWith(assigned);
        }
        
        // return resulting set
        return result;
    }
}