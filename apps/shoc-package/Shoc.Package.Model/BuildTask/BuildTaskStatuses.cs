using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Package.Model.BuildTask;

/// <summary>
/// The build task statuses
/// </summary>
public static class BuildTaskStatuses
{
    /// <summary>
    /// The created status 
    /// </summary>
    public const string CREATED = "created";
    
    /// <summary>
    /// The completed status 
    /// </summary>
    public const string COMPLETED = "completed";
    
    /// <summary>
    /// The failed status 
    /// </summary>
    public const string FAILED = "failed";
    
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
        return typeof(BuildTaskStatuses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}