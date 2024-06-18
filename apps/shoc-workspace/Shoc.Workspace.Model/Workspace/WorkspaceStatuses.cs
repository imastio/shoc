using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Workspace.Model.Workspace;

/// <summary>
/// The known workspace statuses
/// </summary>
public class WorkspaceStatuses
{
    /// <summary>
    /// The workspace is active
    /// </summary>
    public const string ACTIVE = "active";

    /// <summary>
    /// The workspace is archived
    /// </summary>
    public const string ARCHIVED = "archived";
    
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
        return typeof(WorkspaceStatuses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}