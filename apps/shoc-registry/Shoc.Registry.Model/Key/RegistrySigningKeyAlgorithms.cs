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
    /// The RS256 algorithm
    /// </summary>
    public const string RS256 = "RS256";
    
    /// <summary>
    /// The RS384 algorithm
    /// </summary>
    public const string RS384 = "RS384";
    
    /// <summary>
    /// The RS512 algorithm
    /// </summary>
    public const string RS512 = "RS512";
    
    /// <summary>
    /// The PS256 algorithm
    /// </summary>
    public const string PS256 = "PS256";
    
    /// <summary>
    /// The PS384 algorithm
    /// </summary>
    public const string PS384 = "PS384";
    
    /// <summary>
    /// The PS512 algorithm
    /// </summary>
    public const string PS512 = "PS512";
    
    /// <summary>
    /// The ES256 algorithm
    /// </summary>
    public const string ES256 = "ES256";
    
    /// <summary>
    /// The ES384 algorithm
    /// </summary>
    public const string ES384 = "ES384";
    
    /// <summary>
    /// The ES512 algorithm
    /// </summary>
    public const string ES512 = "ES512";
    
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