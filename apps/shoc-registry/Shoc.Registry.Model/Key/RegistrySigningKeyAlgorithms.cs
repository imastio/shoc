using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Registry.Model.Key;

/// <summary>
/// The registry signing key algorithms
/// </summary>
public static class RegistrySigningKeyAlgorithms
{
    /// <summary>
    /// The RSA256 algorithm
    /// </summary>
    public const string RSA256 = "RSA256";
    
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
        return typeof(RegistrySigningKeyAlgorithms)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}