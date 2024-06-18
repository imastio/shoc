using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Workspace.Model;

/// <summary>
/// The definitions for accesses of the service
/// </summary>
public class WorkspaceAccesses
{
    /// <summary>
    /// A read access to workspace objects
    /// </summary>
    public const string WORKSPACE_WORKSPACES_READ = "workspace:workspaces:read";

    /// <summary>
    /// The list access to workspace objects
    /// </summary>
    public const string WORKSPACE_WORKSPACES_LIST = "workspace:workspaces:list";

    /// <summary>
    /// A 'create' access to workspace objects
    /// </summary>
    public const string WORKSPACE_WORKSPACES_CREATE = "workspace:workspaces:create";
    
    /// <summary>
    /// An edit access to workspace objects
    /// </summary>
    public const string WORKSPACE_WORKSPACES_EDIT = "workspace:workspaces:edit";
    
    /// <summary>
    /// A delete access to workspace objects
    /// </summary>
    public const string WORKSPACE_WORKSPACES_DELETE = "workspace:workspaces:delete";
    
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
        return typeof(WorkspaceAccesses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}