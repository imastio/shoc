using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Registry.Model.Key;

/// <summary>
/// The types of signing keys
/// </summary>
public static class SigningKeyTypes
{
    /// <summary>
    /// The RSA key type
    /// </summary>
    public const string RSA = "RSA";
    
    /// <summary>
    /// The EC key type
    /// </summary>
    public const string EC = "EC";
    
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
        return typeof(SigningKeyTypes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}