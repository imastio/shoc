using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoc.Job.Model;

/// <summary>
/// The definitions for accesses of the service
/// </summary>
public class JobAccesses
{
    /// <summary>
    /// A read access to job objects
    /// </summary>
    public const string JOB_JOBS_READ = "job:jobs:read";

    /// <summary>
    /// The list access to job objects
    /// </summary>
    public const string JOB_JOBS_LIST = "job:jobs:list";
    
    /// <summary>
    /// The list references access to job object references
    /// </summary>
    public const string JOB_JOBS_LIST_REFERENCES = "job:jobs:list_references";

    /// <summary>
    /// A 'create' access to job objects
    /// </summary>
    public const string JOB_JOBS_CREATE = "job:jobs:create";
    
    /// <summary>
    /// An edit access to job objects
    /// </summary>
    public const string JOB_JOBS_EDIT = "job:jobs:edit";
    
    /// <summary>
    /// The manage access to job objects
    /// </summary>
    public const string JOB_JOBS_MANAGE = "job:jobs:manage";
    
    /// <summary>
    /// A delete access to job objects
    /// </summary>
    public const string JOB_JOBS_DELETE = "job:jobs:delete";
    
    /// <summary>
    /// A read access to labels objects
    /// </summary>
    public const string JOB_LABELS_READ = "job:labels:read";

    /// <summary>
    /// The list access to labels objects
    /// </summary>
    public const string JOB_LABELS_LIST = "job:labels:list";

    /// <summary>
    /// A 'create' access to labels objects
    /// </summary>
    public const string JOB_LABELS_CREATE = "job:labels:create";
    
    /// <summary>
    /// An edit access to labels objects
    /// </summary>
    public const string JOB_LABELS_EDIT = "job:labels:edit";
    
    /// <summary>
    /// The manage access to labels objects
    /// </summary>
    public const string JOB_LABELS_MANAGE = "job:labels:manage";
    
    /// <summary>
    /// A delete access to labels objects
    /// </summary>
    public const string JOB_LABELS_DELETE = "job:labels:delete";
    
    /// <summary>
    /// A read access to git repos objects
    /// </summary>
    public const string JOB_GIT_REPOS_READ = "job:git_repos:read";

    /// <summary>
    /// The list access to git repos objects
    /// </summary>
    public const string JOB_GIT_REPOS_LIST = "job:git_repos:list";

    /// <summary>
    /// A 'create' access to git repos objects
    /// </summary>
    public const string JOB_GIT_REPOS_CREATE = "job:git_repos:create";
    
    /// <summary>
    /// An edit access to git repos objects
    /// </summary>
    public const string JOB_GIT_REPOS_EDIT = "job:git_repos:edit";
    
    /// <summary>
    /// The manage access to git repos objects
    /// </summary>
    public const string JOB_GIT_REPOS_MANAGE = "job:git_repos:manage";
    
    /// <summary>
    /// A delete access to git repo objects
    /// </summary>
    public const string JOB_GIT_REPOS_DELETE = "job:git_repos:delete";
    
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
        return typeof(JobAccesses)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => !f.IsInitOnly && f.IsLiteral && f.FieldType == typeof(string))
            .Select(f => f.GetRawConstantValue() as string)
            .ToHashSet();
    }
}