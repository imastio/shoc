using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Cluster.Model;

/// <summary>
/// The definitions for accesses of the service
/// </summary>
public class ClusterAccesses
{
    /// <summary>
    /// A read access to cluster objects
    /// </summary>
    public const string CLUSTER_CLUSTERS_READ = "cluster:clusters:read";

    /// <summary>
    /// The list access to cluster objects
    /// </summary>
    public const string CLUSTER_CLUSTERS_LIST = "cluster:clusters:list";
    
    /// <summary>
    /// The list references access to cluster object references
    /// </summary>
    public const string CLUSTER_CLUSTERS_LIST_REFERENCES = "cluster:clusters:list_references";

    /// <summary>
    /// A 'create' access to cluster objects
    /// </summary>
    public const string CLUSTER_CLUSTERS_CREATE = "cluster:clusters:create";
    
    /// <summary>
    /// An edit access to cluster objects
    /// </summary>
    public const string CLUSTER_CLUSTERS_EDIT = "cluster:clusters:edit";
    
    /// <summary>
    /// The manage access to cluster objects
    /// </summary>
    public const string CLUSTER_CLUSTERS_MANAGE = "cluster:clusters:manage";
    
    /// <summary>
    /// A delete access to cluster objects
    /// </summary>
    public const string CLUSTER_CLUSTERS_DELETE = "cluster:clusters:delete";
    
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
        return typeof(ClusterAccesses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}