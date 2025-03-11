using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.ObjectAccess.Model;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.ObjectAccess.Job;

/// <summary>
/// Evaluate workspace job access
/// </summary>
public class JobAccessEvaluator : IJobAccessEvaluator
{
    /// <summary>
    /// The access repository
    /// </summary>
    private readonly IWorkspaceAccessRepository workspaceAccessRepository;

    /// <summary>
    /// The job access repository
    /// </summary>
    private readonly IJobAccessRepository jobAccessRepository;
    
    /// <summary>
    /// The permission calculator
    /// </summary>
    private readonly JobPermissionCalculator permissionCalculator;

    /// <summary>
    /// Creates new access evaluator module
    /// </summary>
    /// <param name="workspaceAccessRepository">The access repository</param>
    /// <param name="permissionCalculator">The permission calculator</param>
    /// <param name="jobAccessRepository">The job access repository</param>
    public JobAccessEvaluator(IWorkspaceAccessRepository workspaceAccessRepository, JobPermissionCalculator permissionCalculator, IJobAccessRepository jobAccessRepository)
    {
        this.workspaceAccessRepository = workspaceAccessRepository;
        this.permissionCalculator = permissionCalculator;
        this.jobAccessRepository = jobAccessRepository;
    }

    /// <summary>
    /// Gets the permissions of the given user to the given job in the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <returns></returns>
    public async Task<ISet<string>> GetPermissions(string userId, string workspaceId, string jobId)
    {
        // check if job is empty
        if (string.IsNullOrWhiteSpace(jobId))
        {
            return new HashSet<string>();
        }
        
        // try load the access model
        var workspaceRoles = (await this.workspaceAccessRepository.GetRoles(workspaceId, userId)).Select(item => item.Role).ToHashSet();

        // if user does not have any role in the workspace then no permissions on job
        if (workspaceRoles.Count == 0)
        {
            return new HashSet<string>();
        }
        
        // get the job reference
        var reference = await this.jobAccessRepository.GetAccessReferenceById(workspaceId, jobId);

        // if no object found then no permissions can be granted
        if (reference == null)
        {
            return new HashSet<string>();
        }
        
        // get all the granted permissions
        var granted = this.permissionCalculator.Calculate(userId, reference, workspaceRoles);

        // return resulting set
        return granted;
    }
    
    /// <summary>
    /// Evaluate workspace job access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    public async Task<AccessEvaluationResult> Evaluate(string userId, string workspaceId, string jobId, params string[] permissions)
    {   
        // get all the granted permissions
        var granted = await this.GetPermissions(userId, workspaceId, jobId);
        
        // reject permissions that are not in the granted list
        var rejected = permissions.Where(permission => !granted.Contains(permission)).ToHashSet();

        // deny if any of permissions is rejected 
        if (rejected.Count > 0)
        {
            return new AccessEvaluationResult
            {
                Result = EvaluationResultCodes.ACCESS_DENIED,
                RejectedPermissions = rejected
            };
        }
        
        // successful access grant
        return new AccessEvaluationResult
        {
            Result = EvaluationResultCodes.ACCESS_GRANTED
        };
    }

    /// <summary>
    /// Evaluate workspace job access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="jobId">The job id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    public async Task Ensure(string userId, string workspaceId, string jobId, params string[] permissions)
    {
        // evaluate and get the result
        var evaluationResult = await this.Evaluate(userId, workspaceId, jobId, permissions);

        // access granted
        if (evaluationResult.Result == EvaluationResultCodes.ACCESS_GRANTED)
        {
            return;
        }

        throw ErrorDefinition.Access().AsException();
    }
}