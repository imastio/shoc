using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Job.Model.WorkspaceJobTask;
using Shoc.ObjectAccess.Cluster;
using Shoc.ObjectAccess.Job;
using Shoc.ObjectAccess.Model.Job;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Package;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Job.Services;

/// <summary>
/// The workspace job tasks service
/// </summary>
public class WorkspaceJobTaskService : WorkspaceJobServiceBase
{
    /// <summary>
    /// The job task service
    /// </summary>
    protected readonly JobTaskService jobTaskService;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="jobSubmissionService">The job submission service</param>
    /// <param name="jobService">The job service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    /// <param name="packageAccessEvaluator">The package access evaluator</param>
    /// <param name="clusterAccessEvaluator">The cluster access evaluator</param>
    /// <param name="jobAccessEvaluator">The job access evaluator</param>
    /// <param name="jobTaskService">The job task service</param>
    public WorkspaceJobTaskService(JobSubmissionService jobSubmissionService, JobService jobService, IWorkspaceAccessEvaluator workspaceAccessEvaluator, IPackageAccessEvaluator packageAccessEvaluator, IClusterAccessEvaluator clusterAccessEvaluator, IJobAccessEvaluator jobAccessEvaluator, JobTaskService jobTaskService)
        : base(jobSubmissionService, jobService, workspaceAccessEvaluator, packageAccessEvaluator, clusterAccessEvaluator, jobAccessEvaluator)
    {
        this.jobTaskService = jobTaskService;
    }
    
    /// <summary>
    /// Gets all objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<WorkspaceJobTaskModel>> GetAll(string userId, string workspaceId, string jobId)
    {
        // ensure we have a permissions
        await this.workspaceAccessEvaluator.Ensure(
            userId, 
            workspaceId, 
            WorkspacePermissions.WORKSPACE_VIEW, 
            WorkspacePermissions.WORKSPACE_LIST_JOBS
        );
        
        // ensure we have job permission
        await this.jobAccessEvaluator.Ensure(userId, workspaceId, jobId, JobPermissions.JOB_VIEW);

        // load items
        var items = await this.jobTaskService.GetAllExtended(workspaceId, jobId);
        
        // map and return the result
        return items.Select(MapJobTask);
    }
    
    /// <summary>
    /// Gets object by id
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceJobTaskModel> GetById(string userId, string workspaceId, string jobId, string id)
    {
        // ensure we have a necessary permissions
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, 
            WorkspacePermissions.WORKSPACE_VIEW, 
            WorkspacePermissions.WORKSPACE_LIST_JOBS);

        // ensure we have job permission
        await this.jobAccessEvaluator.Ensure(userId, workspaceId, jobId, JobPermissions.JOB_VIEW);
        
        // gets the item by id
        var item = await this.jobTaskService.GetExtendedById(workspaceId, jobId, id);
        
        // map and return the result
        return MapJobTask(item);
    }
    
    /// <summary>
    /// Gets object by sequence
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceJobTaskModel> GetBySequence(string userId, string workspaceId, string jobId, long sequence)
    {
        // ensure we have a necessary permissions
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, 
            WorkspacePermissions.WORKSPACE_VIEW, 
            WorkspacePermissions.WORKSPACE_LIST_JOBS);

        // ensure we have job permission
        await this.jobAccessEvaluator.Ensure(userId, workspaceId, jobId, JobPermissions.JOB_VIEW);
        
        // gets the item by id
        var item = await this.jobTaskService.GetExtendedBySequence(workspaceId, jobId, sequence);
        
        // map and return the result
        return MapJobTask(item);
    }
    
    /// <summary>
    /// Gets logs object by sequence
    /// </summary>
    /// <returns></returns>
    public async Task<Stream> GetLogsBySequence(string userId, string workspaceId, string jobId, long sequence)
    {
        // ensure we have a necessary permissions
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, 
            WorkspacePermissions.WORKSPACE_VIEW, 
            WorkspacePermissions.WORKSPACE_LIST_JOBS);

        // ensure we have job permission
        await this.jobAccessEvaluator.Ensure(userId, workspaceId, jobId, JobPermissions.JOB_VIEW);
        
        // gets the item by id
        var stream = await this.jobTaskService.GetLogsBySequence(workspaceId, jobId, sequence);
        
        // map and return the result
        return stream;
    }
}