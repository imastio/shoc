using System.Threading.Tasks;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.WorkspaceJob;
using Shoc.ObjectAccess.Model.Package;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Package;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Job.Services;

/// <summary>
/// The workspace job service
/// </summary>
public class WorkspaceJobSubmissionService : WorkspaceJobServiceBase
{
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="jobSubmissionService">The job submission service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    /// <param name="packageAccessEvaluator">The package access evaluator</param>
    public WorkspaceJobSubmissionService(JobSubmissionService jobSubmissionService, IWorkspaceAccessEvaluator workspaceAccessEvaluator, IPackageAccessEvaluator packageAccessEvaluator)
        : base(jobSubmissionService, workspaceAccessEvaluator, packageAccessEvaluator)
    {
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceJobCreatedModel> Create(string userId, string workspaceId, JobSubmissionInput input)
    {
        // ensure have required access to workspace
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_VIEW,
            WorkspacePermissions.WORKSPACE_LIST_PACKAGES,
            WorkspacePermissions.WORKSPACE_LIST_CLUSTERS,
            WorkspacePermissions.WORKSPACE_LIST_JOBS,
            WorkspacePermissions.WORKSPACE_SUBMIT_JOB);

        // ensure having the required access to package
        await this.packageAccessEvaluator.Ensure(
            userId,
            workspaceId,
            input.Manifest?.PackageId,
            PackagePermissions.PACKAGE_VIEW,
            PackagePermissions.PACKAGE_USE);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        input.UserId = userId;

        // create the object
        var result = await this.jobSubmissionService.Create(workspaceId, input);
        
        // build result
        return new WorkspaceJobCreatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId,
            LocalId = result.LocalId
        };
    }
}