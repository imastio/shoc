using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.Job.Model.Label;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;

namespace Shoc.Job.Services;

/// <summary>
/// The workspace label service
/// </summary>
public class WorkspaceLabelService
{
    /// <summary>
    /// The label service
    /// </summary>
    private readonly LabelService labelService;

    /// <summary>
    /// The access evaluator
    /// </summary>
    private readonly IWorkspaceAccessEvaluator accessEvaluator;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="labelService">The label service</param>
    /// <param name="accessEvaluator">The access evaluator</param>
    public WorkspaceLabelService(LabelService labelService, IWorkspaceAccessEvaluator accessEvaluator)
    {
        this.labelService = labelService;
        this.accessEvaluator = accessEvaluator;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<LabelModel>> GetAll(string userId, string workspaceId)
    {
        // load items
        var items = await this.labelService.GetAll(workspaceId);
        
        // ensure we have a permission to view workspace secrets
        await this.accessEvaluator.Ensure(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_LABELS);

        // map and return the result
        return items;
    }

    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<LabelModel> Create(string userId, string workspaceId, LabelCreateModel input)
    {
        // ensure have required access
        await this.accessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_VIEW,
            WorkspacePermissions.WORKSPACE_LIST_LABELS,
            WorkspacePermissions.WORKSPACE_CREATE_LABEL);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // create the object
        return await this.labelService.Create(workspaceId, input);
    }
    
    /// <summary>
    /// Creates the object with given input
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The creation input</param>
    /// <returns></returns>
    public async Task<IEnumerable<LabelModel>> Ensure(string userId, string workspaceId, LabelsEnsureModel input)
    {
        // ensure have required access
        await this.accessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_VIEW,
            WorkspacePermissions.WORKSPACE_LIST_LABELS,
            WorkspacePermissions.WORKSPACE_CREATE_LABEL);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // create the object
        return await this.labelService.Ensure(workspaceId, input);
    }

    /// <summary>
    /// Deletes the object by id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The id of the object</param>
    /// <returns></returns>
    public async Task<LabelModel> DeleteById(string userId, string workspaceId, string id)
    {
        // ensure have required access
        await this.accessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_VIEW,
            WorkspacePermissions.WORKSPACE_LIST_LABELS,
            WorkspacePermissions.WORKSPACE_DELETE_PACKAGE);
      
        // delete the object
        var result = await this.labelService.DeleteById(workspaceId, id);
        
        // return deleted object
        return result;
    }
}