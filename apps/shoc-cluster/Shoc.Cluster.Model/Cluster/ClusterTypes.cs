using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Cluster.Model.Cluster;

/// <summary>
/// The cluster types
/// </summary>
public static class ClusterTypes
{
    /// <summary>
    /// The k8s type 
    /// </summary>
    public const string K8S = "k8s";

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
        return typeof(ClusterTypes)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}