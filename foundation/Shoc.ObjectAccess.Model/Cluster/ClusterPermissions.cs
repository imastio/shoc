using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.ObjectAccess.Model.Cluster;

/// <summary>
/// The cluster permissions
/// </summary>
public class ClusterPermissions
{
    /// <summary>
    /// View the cluster
    /// </summary>
    public const string CLUSTER_VIEW = "cluster_view";

    /// <summary>
    /// Use the cluster
    /// </summary>
    public const string CLUSTER_USE = "cluster_use";
    
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
        return typeof(ClusterPermissions)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}