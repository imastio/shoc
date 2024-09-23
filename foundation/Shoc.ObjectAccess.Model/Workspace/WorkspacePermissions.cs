using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.ObjectAccess.Model.Workspace;

/// <summary>
/// The known workspace permissions
/// </summary>
public class WorkspacePermissions
{
    /// <summary>
    /// View the workspace
    /// </summary>
    public const string WORKSPACE_VIEW = "workspace_view";
    
    /// <summary>
    /// Edit the workspace
    /// </summary>
    public const string WORKSPACE_UPDATE = "workspace_update";

    /// <summary>
    /// Delete the workspace
    /// </summary>
    public const string WORKSPACE_DELETE = "workspace_delete";
    
    /// <summary>
    /// View members of the workspace
    /// </summary>
    public const string WORKSPACE_LIST_MEMBERS = "workspace_list_members";
    
    /// <summary>
    /// Update members of the workspace
    /// </summary>
    public const string WORKSPACE_UPDATE_MEMBER = "workspace_update_member";
    
    /// <summary>
    /// Delete members of the workspace
    /// </summary>
    public const string WORKSPACE_DELETE_MEMBER = "workspace_delete_member";
    
    /// <summary>
    /// View invitations of the workspace
    /// </summary>
    public const string WORKSPACE_LIST_INVITATIONS = "workspace_list_invitations";
    
    /// <summary>
    /// Create invitation of the workspace
    /// </summary>
    public const string WORKSPACE_CREATE_INVITATION = "workspace_create_invitation";
    
    /// <summary>
    /// Update invitations of the workspace
    /// </summary>
    public const string WORKSPACE_UPDATE_INVITATION = "workspace_update_invitation";
    
    /// <summary>
    /// Delete invitations of the workspace
    /// </summary>
    public const string WORKSPACE_DELETE_INVITATION = "workspace_delete_invitation";
    
    /// <summary>
    /// View secrets of the workspace
    /// </summary>
    public const string WORKSPACE_LIST_SECRETS = "workspace_list_secrets";
    
    /// <summary>
    /// View clusters of the workspace
    /// </summary>
    public const string WORKSPACE_LIST_CLUSTERS = "workspace_list_clusters";
    
    /// <summary>
    /// Create cluster of the workspace
    /// </summary>
    public const string WORKSPACE_CREATE_CLUSTER = "workspace_create_cluster";
    
    /// <summary>
    /// Update cluster of the workspace
    /// </summary>
    public const string WORKSPACE_UPDATE_CLUSTER = "workspace_update_cluster";
    
    /// <summary>
    /// Delete cluster of the workspace
    /// </summary>
    public const string WORKSPACE_DELETE_CLUSTER = "workspace_delete_cluster";
    
    /// <summary>
    /// Create secret of the workspace
    /// </summary>
    public const string WORKSPACE_CREATE_SECRET = "workspace_create_secret";
    
    /// <summary>
    /// Update secret of the workspace
    /// </summary>
    public const string WORKSPACE_UPDATE_SECRET = "workspace_update_secret";
    
    /// <summary>
    /// Delete secret of the workspace
    /// </summary>
    public const string WORKSPACE_DELETE_SECRET = "workspace_delete_secret";
    
    /// <summary>
    /// View user secrets of the workspace
    /// </summary>
    public const string WORKSPACE_LIST_USER_SECRETS = "workspace_list_user_clusters";
    
    /// <summary>
    /// Create user secret of the workspace
    /// </summary>
    public const string WORKSPACE_CREATE_USER_SECRET = "workspace_create_user_cluster";
    
    /// <summary>
    /// Update user secret of the workspace
    /// </summary>
    public const string WORKSPACE_UPDATE_USER_SECRET = "workspace_update_user_cluster";
    
    /// <summary>
    /// Delete user secret of the workspace
    /// </summary>
    public const string WORKSPACE_DELETE_USER_SECRET = "workspace_delete_user_cluster";
        
    /// <summary>
    /// Get and initialize all the constants
    /// </summary>
    public static readonly ISet<string> ALL = GetAll();

    /// <summary>
    /// Gets all the constant values
    /// </summary>
    /// <returns></returns>
    private static ISet<string> GetAll()
    {
        return typeof(WorkspacePermissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}