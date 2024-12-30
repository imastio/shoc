using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.ObjectAccess.Model.Package;

/// <summary>
/// The known workspace package permissions
/// </summary>
public class PackagePermissions
{
    /// <summary>
    /// View the package
    /// </summary>
    public const string PACKAGE_VIEW = "package_view";

    /// <summary>
    /// Use the package
    /// </summary>
    public const string PACKAGE_USE = "package_use";
    
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
        return typeof(PackagePermissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}