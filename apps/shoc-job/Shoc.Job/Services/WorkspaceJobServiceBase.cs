using Shoc.Job.Model.Job;
using Shoc.Job.Model.JobTask;
using Shoc.Job.Model.WorkspaceJob;
using Shoc.Job.Model.WorkspaceJobTask;
using Shoc.ObjectAccess.Cluster;
using Shoc.ObjectAccess.Job;
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
    /// The job service
    /// </summary>
    protected readonly JobService jobService;

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
    /// The job access evaluator
    /// </summary>
    protected readonly IJobAccessEvaluator jobAccessEvaluator;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="jobSubmissionService">The job submission service</param>
    /// <param name="jobService">The job service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    /// <param name="packageAccessEvaluator">The package access evaluator</param>
    /// <param name="clusterAccessEvaluator">The cluster access evaluator</param>
    /// <param name="jobAccessEvaluator">The job access evaluator</param>
    protected WorkspaceJobServiceBase(JobSubmissionService jobSubmissionService, JobService jobService, IWorkspaceAccessEvaluator workspaceAccessEvaluator, IPackageAccessEvaluator packageAccessEvaluator, IClusterAccessEvaluator clusterAccessEvaluator, IJobAccessEvaluator jobAccessEvaluator)
    {
        this.jobSubmissionService = jobSubmissionService;
        this.jobService = jobService;
        this.workspaceAccessEvaluator = workspaceAccessEvaluator;
        this.packageAccessEvaluator = packageAccessEvaluator;
        this.clusterAccessEvaluator = clusterAccessEvaluator;
        this.jobAccessEvaluator = jobAccessEvaluator;
    }

    /// <summary>
    /// Maps an extended model of job to a workspace job model
    /// </summary>
    /// <param name="item">The item to map</param>
    /// <returns></returns>
    protected static WorkspaceJobModel MapJob(JobExtendedModel item)
    {
        return new WorkspaceJobModel
        {
            Id = item.Id,
            WorkspaceId = item.WorkspaceId,
            WorkspaceName = item.WorkspaceName,
            LocalId = item.LocalId,
            ClusterId = item.ClusterId,
            ClusterName = item.ClusterName,
            UserId = item.UserId,
            UserFullName = item.UserFullName,
            Scope = item.Scope,
            Manifest = item.Manifest,
            Namespace = item.Namespace,
            TotalTasks = item.TotalTasks,
            SucceededTasks = item.SucceededTasks,
            FailedTasks = item.FailedTasks,
            CancelledTasks = item.CancelledTasks,
            CompletedTasks = item.CompletedTasks,
            Status = item.Status,
            Message = item.Message,
            PendingAt = item.PendingAt,
            RunningAt = item.RunningAt,
            CompletedAt = item.CompletedAt,
            Created = item.Created,
            Updated = item.Updated
        };
    }
    
    /// <summary>
    /// Maps an extended model of job to a workspace job task model
    /// </summary>
    /// <param name="item">The item to map</param>
    /// <returns></returns>
    protected static WorkspaceJobTaskModel MapJobTask(JobTaskExtendedModel item)
    {
        return new WorkspaceJobTaskModel
        {
            Id = item.Id,
            WorkspaceId = item.WorkspaceId,
            WorkspaceName = item.WorkspaceName,
            JobId = item.JobId,
            Sequence = item.Sequence,
            ClusterId = item.ClusterId,
            ClusterName = item.ClusterName,
            PackageId = item.PackageId,
            PackageImage = item.PackageImage,
            UserId = item.UserId,
            UserFullName = item.UserFullName,
            Type = item.Type,
            Runtime = item.Runtime,
            Args = item.Args,
            ArrayReplicas = item.ArrayReplicas,
            ArrayIndexer = item.ArrayIndexer,
            ArrayCounter = item.ArrayCounter,
            MemoryRequested = item.MemoryRequested,
            CpuRequested = item.CpuRequested,
            NvidiaGpuRequested = item.NvidiaGpuRequested,
            AmdGpuRequested = item.AmdGpuRequested,
            Spec = item.Spec,
            Status = item.Status,
            Message = item.Message,
            PendingAt = item.PendingAt,
            RunningAt = item.RunningAt,
            CompletedAt = item.CompletedAt,
            Created = item.Created,
            Updated = item.Updated
        };
    }
}