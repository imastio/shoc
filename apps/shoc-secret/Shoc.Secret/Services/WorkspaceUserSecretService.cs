using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shoc.ObjectAccess.Model.Workspace;
using Shoc.ObjectAccess.Workspace;
using Shoc.Secret.Model.UserSecret;
using Shoc.Secret.Model.WorkspaceUserSecret;

namespace Shoc.Secret.Services;

/// <summary>
/// The workspace user secret service
/// </summary>
public class WorkspaceUserSecretService
{
    /// <summary>
    /// the default mask for secret value
    /// </summary>
    private const string SECRET_VALUE_MASK = "*********";
    
    /// <summary>
    /// The user secret service
    /// </summary>
    private readonly UserSecretService userSecretService;
    
    /// <summary>
    /// The workspace access evaluator
    /// </summary>
    private readonly IWorkspaceAccessEvaluator workspaceAccessEvaluator;

    /// <summary>
    /// The workspace secret service
    /// </summary>
    /// <param name="userSecretService">The user secret service</param>
    /// <param name="workspaceAccessEvaluator">The access evaluator</param>
    public WorkspaceUserSecretService(UserSecretService userSecretService, IWorkspaceAccessEvaluator workspaceAccessEvaluator)
    {
        this.userSecretService = userSecretService;
        this.workspaceAccessEvaluator = workspaceAccessEvaluator;
    }
    
    /// <summary>
    /// Gets all the objects
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<WorkspaceUserSecretModel>> GetAll(string userId, string workspaceId)
    {
        // load items
        var items = await this.userSecretService.GetAllExtended(workspaceId, userId);
        
        // ensure we have a permission to view workspace secrets
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_USER_SECRETS);

        // map and return the result
        return items.Select(Map);
    }
    
    /// <summary>
    /// Counts all objects
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceUserSecretCountModel> CountAll(string userId, string workspaceId)
    {
        // count the objects
        var count = await this.userSecretService.CountAll(workspaceId, userId);
        
        // ensure we have a permission to view workspace secrets
        await this.workspaceAccessEvaluator.Ensure(userId, workspaceId, WorkspacePermissions.WORKSPACE_VIEW, WorkspacePermissions.WORKSPACE_LIST_USER_SECRETS);

        // map and return the result
        return new WorkspaceUserSecretCountModel
        {
            Count = count.Count
        };
    }
    
    /// <summary>
    /// Creates a new object
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceUserSecretCreatedModel> Create(string userId, string workspaceId, WorkspaceUserSecretCreateModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_USER_SECRETS,
            WorkspacePermissions.WORKSPACE_CREATE_USER_SECRET);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        input.UserId = userId;

        // create the object
        var result = await this.userSecretService.Create(workspaceId, userId, new UserSecretCreateModel
        {
            WorkspaceId = input.WorkspaceId,
            UserId = input.UserId,
            Name = input.Name,
            Description = input.Description,
            Disabled = input.Disabled,
            Encrypted = input.Encrypted,
            Value = input.Value
        });
        
        // return existing object
        return new WorkspaceUserSecretCreatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId,
            UserId = result.UserId
        };
    }
    
    /// <summary>
    /// Updates the existing object
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceUserSecretUpdatedModel> UpdateById(string userId, string workspaceId, string id, WorkspaceUserSecretUpdateModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_USER_SECRETS,
            WorkspacePermissions.WORKSPACE_UPDATE_USER_SECRET);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        input.UserId = userId;
        input.Id = id;

        // update the object
        var result = await this.userSecretService.UpdateById(workspaceId, userId, id, new UserSecretUpdateModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            UserId = input.UserId,
            Name = input.Name,
            Description = input.Description,
            Disabled = input.Disabled
        });
        
        // return existing object
        return new WorkspaceUserSecretUpdatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId,
            UserId = result.UserId
        };
    }
    
    /// <summary>
    /// Updates the existing object value
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceUserSecretUpdatedModel> UpdateValueById(string userId, string workspaceId, string id, WorkspaceUserSecretValueUpdateModel input)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_USER_SECRETS,
            WorkspacePermissions.WORKSPACE_UPDATE_USER_SECRET);
        
        // ensure referring to the correct object
        input.WorkspaceId = workspaceId;
        input.UserId = userId;
        input.Id = id;

        // update the object
        var result = await this.userSecretService.UpdateValueById(workspaceId, userId, id, new UserSecretValueUpdateModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            UserId = input.UserId,
            Encrypted = input.Encrypted,
            Value = input.Value
        });
        
        // return existing object
        return new WorkspaceUserSecretUpdatedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId,
            UserId = input.UserId
        };
    }
    
    /// <summary>
    /// Deletes the existing object
    /// </summary>
    /// <returns></returns>
    public async Task<WorkspaceUserSecretDeletedModel> DeleteById(string userId, string workspaceId, string id)
    {
        // ensure have required access
        await this.workspaceAccessEvaluator.Ensure(
            userId,
            workspaceId,
            WorkspacePermissions.WORKSPACE_LIST_USER_SECRETS,
            WorkspacePermissions.WORKSPACE_DELETE_USER_SECRET);
      
        // delete the object
        var result = await this.userSecretService.DeleteById(workspaceId, userId, id);
        
        // return deleted object
        return new WorkspaceUserSecretDeletedModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId,
            UserId = result.UserId
        };
    }
    
    /// <summary>
    /// Maps the secret model into a workspace secret model
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    protected static WorkspaceUserSecretModel Map(UserSecretExtendedModel input)
    {
        return new WorkspaceUserSecretModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            WorkspaceName = input.WorkspaceName,
            UserId = input.UserId,
            UserFullName = input.UserFullName,
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