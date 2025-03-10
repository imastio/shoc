using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.ObjectAccess.Model;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.ObjectAccess.Cluster;

/// <summary>
/// Evaluate workspace cluster access
/// </summary>
public class ClusterAccessEvaluator : IClusterAccessEvaluator
{
    /// <summary>
    /// The access repository
    /// </summary>
    private readonly IWorkspaceAccessRepository workspaceAccessRepository;
    
    /// <summary>
    /// The permission calculator
    /// </summary>
    private readonly ClusterPermissionCalculator permissionCalculator;

    /// <summary>
    /// Creates new access evaluator module
    /// </summary>
    /// <param name="workspaceAccessRepository">The access repository</param>
    /// <param name="permissionCalculator">The permission calculator</param>
    public ClusterAccessEvaluator(IWorkspaceAccessRepository workspaceAccessRepository, ClusterPermissionCalculator permissionCalculator)
    {
        this.workspaceAccessRepository = workspaceAccessRepository;
        this.permissionCalculator = permissionCalculator;
    }

    /// <summary>
    /// Gets the permissions of the given user to the given cluster in the workspace
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="clusterId">The cluster id</param>
    /// <returns></returns>
    public async Task<ISet<string>> GetPermissions(string userId, string workspaceId, string clusterId)
    {
        // check if package is empty
        if (string.IsNullOrWhiteSpace(clusterId))
        {
            return new HashSet<string>();
        }
        
        // try load the access model
        var workspaceRoles = (await this.workspaceAccessRepository.GetRoles(workspaceId, userId)).Select(item => item.Role).ToHashSet();

        // if user does not have any role in the workspace then no permissions on package
        if (workspaceRoles.Count == 0)
        {
            return new HashSet<string>();
        }
        
        // get all the granted permissions
        var granted = this.permissionCalculator.Calculate(workspaceRoles.ToArray());

        // return resulting set
        return granted;
    }
    
    /// <summary>
    /// Evaluate workspace cluster access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="clusterId">The cluster id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    public async Task<AccessEvaluationResult> Evaluate(string userId, string workspaceId, string clusterId, params string[] permissions)
    {   
        // get all the granted permissions
        var granted = await this.GetPermissions(userId, workspaceId, clusterId);
        
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
    /// Evaluate workspace cluster access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="clusterId">The cluster id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    public async Task Ensure(string userId, string workspaceId, string clusterId, params string[] permissions)
    {
        // evaluate and get the result
        var evaluationResult = await this.Evaluate(userId, workspaceId, clusterId, permissions);

        // access granted
        if (evaluationResult.Result == EvaluationResultCodes.ACCESS_GRANTED)
        {
            return;
        }

        throw ErrorDefinition.Access().AsException();
    }
}