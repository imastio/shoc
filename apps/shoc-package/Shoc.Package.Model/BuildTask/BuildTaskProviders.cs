using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Package.Model.BuildTask;

/// <summary>
/// The build task providers
/// </summary>
public static class BuildTaskProviders
{
    /// <summary>
    /// The build process is done remotely
    /// </summary>
    public const string REMOTE = "remote";

    /// <summary>
    /// The build process is done locally
    /// </summary>
    public const string LOCAL = "local";
    
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
        return typeof(BuildTaskProviders)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}