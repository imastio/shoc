using Shoc.ObjectAccess.Cluster;
using Shoc.ObjectAccess.Package;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Job.Services;

/// <summary>
/// The workspace job service base
/// </summary>
public class WorkspaceJobServiceBase
{
    /// <summary>
    /// The job submission service
    /// </summary>
    protected readonly JobSubmissionService jobSubmissionService;

    /// <summary>
    /// The workspace access evaluator
    /// </summary>
    protected readonly IWorkspaceAccessEvaluator workspaceAccessEvaluator;

    /// <summary>
    /// The package access evaluator
    /// </summary>
    protected readonly IPackageAccessEvaluator packageAccessEvaluator;

    /// <summary>
    /// The cluster access evaluator
    /// </summary>
    protected readonly IClusterAccessEvaluator clusterAccessEvaluator;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="jobSubmissionService">The job submission service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    /// <param name="packageAccessEvaluator">The package access evaluator</param>
    /// <param name="clusterAccessEvaluator">The cluster access evaluator</param>
    protected WorkspaceJobServiceBase(JobSubmissionService jobSubmissionService, IWorkspaceAccessEvaluator workspaceAccessEvaluator, IPackageAccessEvaluator packageAccessEvaluator, IClusterAccessEvaluator clusterAccessEvaluator)
    {
        this.jobSubmissionService = jobSubmissionService;
        this.workspaceAccessEvaluator = workspaceAccessEvaluator;
        this.packageAccessEvaluator = packageAccessEvaluator;
        this.clusterAccessEvaluator = clusterAccessEvaluator;
    }
}