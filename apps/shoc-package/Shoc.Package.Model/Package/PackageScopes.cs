using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Package.Model.Package;

/// <summary>
/// The package scopes
/// </summary>
public static class PackageScopes
{
    /// <summary>
    /// The workspace scope 
    /// </summary>
    public const string WORKSPACE = "workspace";

    /// <summary>
    /// The user scope 
    /// </summary>
    public const string USER = "user";
    
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
        return typeof(PackageScopes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}