using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Registry.Model.Registry;

/// <summary>
/// The registry usage scopes
/// </summary>
public static class RegistryUsageScopes
{
    /// <summary>
    /// The global scope 
    /// </summary>
    public const string GLOBAL = "global";

    /// <summary>
    /// The workspace scope 
    /// </summary>
    public const string WORKSPACE = "workspace";
    
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
        return typeof(RegistryUsageScopes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}