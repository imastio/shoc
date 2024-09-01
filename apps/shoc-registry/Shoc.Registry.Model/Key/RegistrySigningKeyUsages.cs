using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Registry.Model.Key;

/// <summary>
/// The registry signing key usages
/// </summary>
public static class RegistrySigningKeyUsages
{
    /// <summary>
    /// The signing usage
    /// </summary>
    public const string SIGNING = "signing";
    
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
        return typeof(RegistrySigningKeyUsages)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}