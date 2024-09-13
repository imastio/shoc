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
    public const string WORKSPACE_EDIT = "workspace_edit";

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
    /// View clusters of the workspace
    /// </summary>
    public const string WORKSPACE_LIST_CLUSTERS = "workspace_list_clusters";
        
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