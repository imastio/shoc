using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Registry.Model.Credential;

/// <summary>
/// The registry credential sources
/// </summary>
public static class RegistryCredentialSources
{
    /// <summary>
    /// The manual source 
    /// </summary>
    public const string MANUAL = "manual";

    /// <summary>
    /// The generated source
    /// </summary>
    public const string GENERATED = "generated";
    
    /// <summary>
    /// The integration source
    /// </summary>
    public const string INTEGRATION = "integration";
    
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
        return typeof(RegistryCredentialSources)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}