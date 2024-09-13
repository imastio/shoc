using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Workspace.Model.Workspace;

/// <summary>
/// The known workspace types
/// </summary>
public class WorkspaceTypes
{
    /// <summary>
    /// The individual workspace account
    /// </summary>
    public const string INDIVIDUAL = "individual";

    /// <summary>
    /// The organization workspace account
    /// </summary>
    public const string ORGANIZATION = "organization";
    
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
        return typeof(WorkspaceTypes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}