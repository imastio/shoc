using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Registry.Model.Registry;

/// <summary>
/// The registry statuses
/// </summary>
public static class RegistryStatuses
{
    /// <summary>
    /// The active status 
    /// </summary>
    public const string ACTIVE = "active";

    /// <summary>
    /// The archived status 
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
        return typeof(RegistryStatuses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}