using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Job.Model.JobTask;

/// <summary>
/// The job task statuses
/// </summary>
public static class JobTaskStatuses
{
    /// <summary>
    /// The task is created but not yet picked up by workers
    /// </summary>
    public const string CREATED = "created";
    
    /// <summary>
    /// The task is pending to be scheduled on the cluster 
    /// </summary>
    public const string PENDING = "pending";

    /// <summary>
    /// The task is running
    /// </summary>
    public const string RUNNING = "running";
    
    /// <summary>
    /// The task is completed with success
    /// </summary>
    public const string SUCCEEDED = "succeeded";
    
    /// <summary>
    /// The task is completed with failure
    /// </summary>
    public const string FAILED = "failed";
    
    /// <summary>
    /// The task is cancelled 
    /// </summary>
    public const string CANCELLED = "cancelled";
    
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
        return typeof(JobTaskStatuses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}