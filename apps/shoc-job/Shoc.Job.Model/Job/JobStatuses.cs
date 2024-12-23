using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Job.Model.Job;

/// <summary>
/// The job statuses
/// </summary>
public static class JobStatuses
{
    /// <summary>
    /// The job is created but none of its tasks is submitted
    /// </summary>
    public const string CREATED = "created";
    
    /// <summary>
    /// The tasks of the job are scheduled but not picked up yet 
    /// </summary>
    public const string PENDING = "pending";

    /// <summary>
    /// The job has at least one task that is running
    /// </summary>
    public const string RUNNING = "running";
    
    /// <summary>
    /// The job tasks are completed is completed while some are succeeded some are failed
    /// </summary>
    public const string PARTIALLY_SUCCEEDED = "partially_succeeded";
    
    /// <summary>
    /// The job tasks are completed with success
    /// </summary>
    public const string SUCCEEDED = "succeeded";
    
    /// <summary>
    /// The job tasks are completed with failure
    /// </summary>
    public const string FAILED = "failed";
    
    /// <summary>
    /// The job tasks are completed, but some are cancelled 
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
        return typeof(JobStatuses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}