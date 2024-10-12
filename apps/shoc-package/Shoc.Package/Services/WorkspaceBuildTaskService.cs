using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;
using Shoc.Package.Model.BuildTask;

namespace Shoc.Package.Services;

/// <summary>
/// The workspace build task service
/// </summary>
public class WorkspaceBuildTaskService
{
    /// <summary>
    /// The build task service
    /// </summary>
    private readonly BuildTaskService buildTaskService;

    /// <summary>
    /// The workspace access evaluator
    /// </summary>
    private readonly IWorkspaceAccessEvaluator workspaceAccessEvaluator;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="buildTaskService">The build task service</param>
    /// <param name="workspaceAccessEvaluator">The workspace access evaluator</param>
    public WorkspaceBuildTaskService(BuildTaskService buildTaskService, IWorkspaceAccessEvaluator workspaceAccessEvaluator)
    {
        this.buildTaskService = buildTaskService;
        this.workspaceAccessEvaluator = workspaceAccessEvaluator;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<BuildTaskModel>> GetAll(string userId, string workspaceId)
    {
        // load items
        var items = await this.buildTaskService.GetAll(workspaceId);
        
        // ensure we have a permission to view workspace secrets
        await this.workspaceAccessEvaluator.Evaluate(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_PACKAGES);

        // map and return the result
        return items;
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <returns></returns>
    public async Task<BuildTaskModel> Create(string userId, string workspaceId, BuildTaskCreateModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_SECRETS,
            WorkspacePermissions.WORKSPACE_BUILD_PACKAGE);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        input.UserId = userId;

        // create the object
        return await this.buildTaskService.Create(workspaceId, input);
    }
    
    /// <summary>
    /// Updates the bundle of the object
    /// </summary>
    /// <returns></returns>
    public async Task<BuildTaskModel> UpdateBundleById(string userId, string workspaceId, string id, Stream file)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_SECRETS,
            WorkspacePermissions.WORKSPACE_BUILD_PACKAGE);
        
        // create the object
        return await this.buildTaskService.UpdateBundleById(userId, workspaceId, id, file);
    }
}