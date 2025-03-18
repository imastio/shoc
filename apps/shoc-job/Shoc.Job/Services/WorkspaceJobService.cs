using System.Linq;
using System.Threading.Tasks;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.WorkspaceJob;
using Shoc.ObjectAccess.Cluster;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Package;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Job.Services;

/// <summary>
/// The workspace job submission service
/// </summary>
public class WorkspaceJobService : WorkspaceJobServiceBase
{
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="jobSubmissionService">The job submission service</param>
    /// <param name="jobService">The job service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    /// <param name="packageAccessEvaluator">The package access evaluator</param>
    /// <param name="clusterAccessEvaluator">The cluster access evaluator</param>
    public WorkspaceJobService(JobSubmissionService jobSubmissionService, JobService jobService, IWorkspaceAccessEvaluator workspaceAccessEvaluator, IPackageAccessEvaluator packageAccessEvaluator, IClusterAccessEvaluator clusterAccessEvaluator)
        : base(jobSubmissionService, jobService, workspaceAccessEvaluator, packageAccessEvaluator, clusterAccessEvaluator)
    {
    }
    
    /// <summary>
    /// Gets page of objects
    /// </summary>
    /// <returns></returns>
    public async Task<JobPageResult<WorkspaceJobModel>> GetPageBy(string userId, string workspaceId, JobFilter filter, int page, int? size)
    {
        // ensure accessing with proper user
        filter.AccessingUserId = userId;
        
        //ensure we have a permissions
        await this.workspaceAccessEvaluator.Ensure(
            userId, 
            workspaceId, 
            WorkspacePermissions.WORKSPACE_VIEW, 
            filter.AccessibleOnly ? WorkspacePermissions.WORKSPACE_LIST_JOBS : WorkspacePermissions.WORKSPACE_LIST_ALL_JOBS
        );

        // load items
        var items = await this.jobService.GetExtendedPageBy(workspaceId, filter, page, size);

        // map and return the result
        return new JobPageResult<WorkspaceJobModel>
        {
            TotalCount = items.TotalCount,
            Items = items.Items.Select(MapJob)
        };
    }
}