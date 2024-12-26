using System.Collections.Generic;
using Shoc.Job.Model.JobGitRepo;
using Shoc.Job.Model.JobLabel;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.Model.Job;

/// <summary>
/// The job definition creation model
/// </summary>
public class JobCreateModel
{
    /// <summary>
    /// The job object
    /// </summary>
    public JobModel Job { get; set; }
    
    /// <summary>
    /// The set of tasks
    /// </summary>
    public IEnumerable<JobTaskModel> Tasks { get; set; }
    
    /// <summary>
    /// The set of labels
    /// </summary>
    public IEnumerable<JobLabelModel> Labels { get; set; }
    
    /// <summary>
    /// The git repo reference
    /// </summary>
    public JobGitRepoModel GitRepo { get; set; }
}