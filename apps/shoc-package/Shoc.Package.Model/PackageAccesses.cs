using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Package.Model;

/// <summary>
/// The definitions for accesses of the service
/// </summary>
public class PackageAccesses
{
    /// <summary>
    /// A read access to package objects
    /// </summary>
    public const string PACKAGE_PACKAGES_READ = "package:packages:read";

    /// <summary>
    /// The list access to package objects
    /// </summary>
    public const string PACKAGE_PACKAGES_LIST = "package:packages:list";
    
    /// <summary>
    /// The list references access to package object references
    /// </summary>
    public const string PACKAGE_PACKAGES_LIST_REFERENCES = "package:packages:list_references";

    /// <summary>
    /// A 'create' access to package objects
    /// </summary>
    public const string PACKAGE_PACKAGES_CREATE = "package:packages:create";
    
    /// <summary>
    /// An edit access to package objects
    /// </summary>
    public const string PACKAGE_PACKAGES_EDIT = "package:packages:edit";
    
    /// <summary>
    /// The manage access to package objects
    /// </summary>
    public const string PACKAGE_PACKAGES_MANAGE = "package:packages:manage";
    
    /// <summary>
    /// A delete access to package objects
    /// </summary>
    public const string PACKAGE_PACKAGES_DELETE = "package:packages:delete";
    
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
        return typeof(PackageAccesses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}