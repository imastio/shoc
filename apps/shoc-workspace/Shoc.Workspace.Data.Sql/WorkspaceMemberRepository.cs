using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Data.Sql;

/// <summary>
/// The workspace member repository
/// </summary>
public class WorkspaceMemberRepository : IWorkspaceMemberRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public WorkspaceMemberRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the members of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public Task<IEnumerable<WorkspaceMemberModel>> GetAll(string workspaceId)
    {
        return this.dataOps.Connect().Query("Workspace.Workspace.Member", "GetAll").ExecuteAsync<WorkspaceMemberModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets all the extended members of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public Task<IEnumerable<WorkspaceMemberExtendedModel>> GetAllExtended(string workspaceId)
    {
        return this.dataOps.Connect().Query("Workspace.Workspace.Member", "GetAllExtended").ExecuteAsync<WorkspaceMemberExtendedModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets the particular membership record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The membership id</param>
    /// <returns></returns>
    public Task<WorkspaceMemberModel> GetById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Member", "GetById").ExecuteAsync<WorkspaceMemberModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
    
    /// <summary>
    /// Gets the particular membership record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public Task<WorkspaceMemberModel> GetByUserId(string workspaceId, string userId)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Member", "GetByUserId").ExecuteAsync<WorkspaceMemberModel>(new
        {
            WorkspaceId = workspaceId,
            UserId = userId
        });
    }

    /// <summary>
    /// Gets the particular extended membership record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The membership id</param>
    /// <returns></returns>
    public Task<WorkspaceMemberExtendedModel> GetExtendedById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Member", "GetExtendedById").ExecuteAsync<WorkspaceMemberExtendedModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
    
    /// <summary>
    /// Gets the particular membership record in the workspace by email
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="email">The member email</param>
    /// <returns></returns>
    public Task<WorkspaceMemberModel> GetByEmail(string workspaceId, string email)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Member", "GetByEmail").ExecuteAsync<WorkspaceMemberModel>(new
        {
            WorkspaceId = workspaceId,
            Email = email
        });
    }

    /// <summary>
    /// Creates new membership in the workspace
    /// </summary>
    /// <param name="input">The workspace membership create input</param>
    /// <returns></returns>
    public Task<WorkspaceMemberModel> Create(WorkspaceMemberCreateModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(WorkspaceObjects.WORKSPACE_MEMBER)?.ToLowerInvariant();

        // perform operation
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Member", "Create").ExecuteAsync<WorkspaceMemberModel>(input);
    }

    /// <summary>
    /// Update the existing membership record
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The workspace membership update input</param>
    /// <returns></returns>
    public Task<WorkspaceMemberModel> UpdateById(string workspaceId, string id, WorkspaceMemberUpdateModel input)
    {
        input.WorkspaceId = workspaceId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Member", "UpdateById").ExecuteAsync<WorkspaceMemberModel>(input);
    }

    /// <summary>
    /// Deletes the member user from the workspace  
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public Task<WorkspaceMemberModel> DeleteById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Member", "DeleteById").ExecuteAsync<WorkspaceMemberModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
}
