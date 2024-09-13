using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Registry.Model.Registry;

/// <summary>
/// The registry provider types
/// </summary>
public static class RegistryProviderTypes
{
    /// <summary>
    /// The shoc provider 
    /// </summary>
    public const string SHOC = "shoc";

    /// <summary>
    /// The docker hub provider 
    /// </summary>
    public const string DOCKER_HUB = "docker_hub";

    /// <summary>
    /// The Azure provider
    /// </summary>
    public const string AZURE = "azure";
    
    /// <summary>
    /// The AWS provider
    /// </summary>
    public const string AWS = "aws";
    
    /// <summary>
    /// The Google Cloud Provider
    /// </summary>
    public const string GCP = "gcp";

    /// <summary>
    /// The GitHub registry provider
    /// </summary>
    public const string GITHUB = "github";
    
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
        return typeof(RegistryProviderTypes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}