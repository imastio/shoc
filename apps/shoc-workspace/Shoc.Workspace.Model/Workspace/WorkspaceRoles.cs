using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Workspace.Model.Workspace;

/// <summary>
/// The known workspace roles
/// </summary>
public class WorkspaceRoles
{
    /// <summary>
    /// The owner of the workspace
    /// </summary>
    public const string OWNER = "owner";

    /// <summary>
    /// The admin of the workspace
    /// </summary>
    public const string ADMIN = "admin";
    
    /// <summary>
    /// The member of the workspace
    /// </summary>
    public const string MEMBER = "member";
    
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
        return typeof(WorkspaceRoles)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}