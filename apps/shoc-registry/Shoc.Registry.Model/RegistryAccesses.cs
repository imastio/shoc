using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Registry.Model;

/// <summary>
/// The definitions for accesses of the service
/// </summary>
public class RegistryAccesses
{
    /// <summary>
    /// A read access to registry objects
    /// </summary>
    public const string REGISTRY_REGISTRIES_READ = "registry:registries:read";

    /// <summary>
    /// The list access to registry objects
    /// </summary>
    public const string REGISTRY_REGISTRIES_LIST = "registry:registries:list";
    
    /// <summary>
    /// The list references access to registry object references
    /// </summary>
    public const string REGISTRY_REGISTRIES_LIST_REFERENCES = "registry:registries:list_references";

    /// <summary>
    /// A 'create' access to registry objects
    /// </summary>
    public const string REGISTRY_REGISTRIES_CREATE = "registry:registries:create";
    
    /// <summary>
    /// An edit access to registry objects
    /// </summary>
    public const string REGISTRY_REGISTRIES_EDIT = "registry:registries:edit";
    
    /// <summary>
    /// The manage access to registry objects
    /// </summary>
    public const string REGISTRY_REGISTRIES_MANAGE = "registry:registries:manage";
    
    /// <summary>
    /// A delete access to registry objects
    /// </summary>
    public const string REGISTRY_REGISTRIES_DELETE = "registry:registries:delete";
    
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
        return typeof(RegistryAccesses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}