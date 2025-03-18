using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;
using Shoc.Secret.Model.Secret;
using Shoc.Secret.Model.WorkspaceSecret;

namespace Shoc.Secret.Services;

/// <summary>
/// The workspace secret service
/// </summary>
public class WorkspaceSecretService
{
    /// <summary>
    /// the default mask for secret value
    /// </summary>
    private const string SECRET_VALUE_MASK = "*********";
    
    /// <summary>
    /// The secret service
    /// </summary>
    private readonly SecretService secretService;
    
    /// <summary>
    /// The workspace access evaluator
    /// </summary>
    private readonly IWorkspaceAccessEvaluator workspaceAccessEvaluator;

    /// <summary>
    /// The workspace secret service
    /// </summary>
    /// <param name="secretService">The secret service</param>
    /// <param name="workspaceAccessEvaluator">The access evaluator</param>
    public WorkspaceSecretService(SecretService secretService, IWorkspaceAccessEvaluator workspaceAccessEvaluator)
    {
        this.secretService = secretService;
        this.workspaceAccessEvaluator = workspaceAccessEvaluator;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<WorkspaceSecretModel>> GetAll(string userId, string workspaceId)
    {
        // load items
        var items = await this.secretService.GetAllExtended(workspaceId);
        
        // ensure we have a permission to view workspace secrets
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_SECRETS);

        // map and return the result
        return items.Select(Map);
    }
    
    /// <summary>
    /// Counts all objects
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceSecretCountModel> CountAll(string userId, string workspaceId)
    {
        // count the objects
        var count = await this.secretService.CountAll(workspaceId);
        
        // ensure we have a permission to view workspace secrets
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_SECRETS);

        // map and return the result
        return new WorkspaceSecretCountModel
        {
            Count = count.Count
        };
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceSecretCreatedModel> Create(string userId, string workspaceId, WorkspaceSecretCreateModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_SECRETS,
            WorkspacePermissions.WORKSPACE_CREATE_SECRET);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;

        // create the object
        var result = await this.secretService.Create(workspaceId, new SecretCreateModel
        {
            WorkspaceId = input.WorkspaceId,
            Name = input.Name,
            Description = input.Description,
            Disabled = input.Disabled,
            Encrypted = input.Encrypted,
            Value = input.Value
        });
        
        // return existing object
        return new WorkspaceSecretCreatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }
    
    /// <summary>
    /// Updates the existing object
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceSecretUpdatedModel> UpdateById(string userId, string workspaceId, string id, WorkspaceSecretUpdateModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_SECRETS,
            WorkspacePermissions.WORKSPACE_UPDATE_SECRET);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        input.Id = id;

        // update the object
        var result = await this.secretService.UpdateById(workspaceId, id, new SecretUpdateModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            Name = input.Name,
            Description = input.Description,
            Disabled = input.Disabled
        });
        
        // return existing object
        return new WorkspaceSecretUpdatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }
    
    /// <summary>
    /// Updates the existing object value
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceSecretUpdatedModel> UpdateValueById(string userId, string workspaceId, string id, WorkspaceSecretValueUpdateModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_SECRETS,
            WorkspacePermissions.WORKSPACE_UPDATE_SECRET);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        input.Id = id;

        // update the object
        var result = await this.secretService.UpdateValueById(workspaceId, id, new SecretValueUpdateModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            Encrypted = input.Encrypted,
            Value = input.Value
        });
        
        // return existing object
        return new WorkspaceSecretUpdatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }
    
    /// <summary>
    /// Deletes the existing object
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceSecretDeletedModel> DeleteById(string userId, string workspaceId, string id)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_SECRETS,
            WorkspacePermissions.WORKSPACE_DELETE_SECRET);
      
        // delete the object
        var result = await this.secretService.DeleteById(workspaceId, id);
        
        // return deleted object
        return new WorkspaceSecretDeletedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId
        };
    }
    
    /// <summary>
    /// Maps the secret model into a workspace secret model
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    protected static WorkspaceSecretModel Map(SecretExtendedModel input)
    {
        return new WorkspaceSecretModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            WorkspaceName = input.WorkspaceName,
            Name = input.Name,
            Description = input.Description,
            Disabled = input.Disabled,
            Encrypted = input.Encrypted,
            Value = input.Encrypted ? SECRET_VALUE_MASK : input.Value,
            Created = input.Created,
            Updated = input.Updated
        };
    }
}