using System;
using System.Linq;
using System.Threading.Tasks;
using Shoc.Core;
using Shoc.ObjectAccess.Model;

namespace Shoc.ObjectAccess.Workspace;

/// <summary>
/// Evaluate workspace access
/// </summary>
public class WorkspaceAccessEvaluator : IWorkspaceAccessEvaluator
{
    /// <summary>
    /// The access repository
    /// </summary>
    private readonly IWorkspaceAccessRepository accessRepository;

    /// <summary>
    /// The permission calculator
    /// </summary>
    private readonly WorkspacePermissionCalculator permissionCalculator;

    /// <summary>
    /// Creates new access evaluator module
    /// </summary>
    /// <param name="accessRepository">The access repository</param>
    /// <param name="permissionCalculator">The permission calculator</param>
    public WorkspaceAccessEvaluator(IWorkspaceAccessRepository accessRepository, WorkspacePermissionCalculator permissionCalculator)
    {
        this.accessRepository = accessRepository;
        this.permissionCalculator = permissionCalculator;
    }
    
    /// <summary>
    /// Evaluate workspace access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    public async Task<AccessEvaluationResult> Evaluate(string userId, string workspaceId, params string[] permissions)
    {
        // try load the access model
        var workspaceRoles = (await this.accessRepository.GetRoles(workspaceId, userId)).Select(item => item.Role).ToHashSet();
        
        // get all the granted permissions
        var granted = this.permissionCalculator.Calculate(workspaceRoles.ToArray());
        
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
    /// Evaluate workspace access based on requested permissions
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="permissions">The requested permissions</param>
    /// <returns></returns>
    public async Task Ensure(string userId, string workspaceId, params string[] permissions)
    {
        // evaluate and get the result
        var evaluationResult = await this.Evaluate(userId, workspaceId, permissions);

        // access granted
        if (evaluationResult.Result == EvaluationResultCodes.ACCESS_GRANTED)
        {
            return;
        }

        throw ErrorDefinition.Access().AsException();
    }
}