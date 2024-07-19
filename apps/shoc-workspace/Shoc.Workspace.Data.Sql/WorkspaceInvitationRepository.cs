using System.Collections.Generic;
using System.Threading.Tasks;
using Imast.DataOps.Api;
using Shoc.Core;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Data.Sql;

/// <summary>
/// The workspace invitation repository
/// </summary>
public class WorkspaceInvitationRepository : IWorkspaceInvitationRepository
{
    /// <summary>
    /// The data operations instance
    /// </summary>
    private readonly DataOperations dataOps;

    /// <summary>
    /// Creates new instance of repository implementation
    /// </summary>
    /// <param name="dataOps">A DataOps instance</param>
    public WorkspaceInvitationRepository(DataOperations dataOps)
    {
        this.dataOps = dataOps;
    }

    /// <summary>
    /// Gets all the invitations of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public Task<IEnumerable<WorkspaceInvitationModel>> GetAll(string workspaceId)
    {
        return this.dataOps.Connect().Query("Workspace.Workspace.Invitation", "GetAll").ExecuteAsync<WorkspaceInvitationModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets all the extended invitations of the workspace
    /// </summary>
    /// <param name="workspaceId">The record id</param>
    /// <returns></returns>
    public Task<IEnumerable<WorkspaceInvitationExtendedModel>> GetAllExtended(string workspaceId)
    {
        return this.dataOps.Connect().Query("Workspace.Workspace.Invitation", "GetAllExtended").ExecuteAsync<WorkspaceInvitationExtendedModel>(new
        {
            WorkspaceId = workspaceId
        });
    }

    /// <summary>
    /// Gets the particular invitation record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public Task<WorkspaceInvitationModel> GetById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Invitation", "GetById").ExecuteAsync<WorkspaceInvitationModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }

    /// <summary>
    /// Gets the particular invitation record in the workspace by email
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="email">The invitation email</param>
    /// <returns></returns>
    public Task<WorkspaceInvitationModel> GetByEmail(string workspaceId, string email)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Invitation", "GetByEmail").ExecuteAsync<WorkspaceInvitationModel>(new
        {
            WorkspaceId = workspaceId,
            Email = email.ToLowerInvariant()
        });
    }

    /// <summary>
    /// Creates new invitation in the workspace
    /// </summary>
    /// <param name="input">The record create input</param>
    /// <returns></returns>
    public Task<WorkspaceInvitationModel> Create(WorkspaceInvitationCreateModel input)
    {
        // generate id if necessary
        input.Id ??= StdIdGenerator.Next(WorkspaceObjects.WORKSPACE_INVITATION)?.ToLowerInvariant();

        // perform operation
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Invitation", "Create").ExecuteAsync<WorkspaceInvitationModel>(input);
    }

    /// <summary>
    /// Update the existing invitation record
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The record update input</param>
    /// <returns></returns>
    public Task<WorkspaceInvitationModel> UpdateById(string workspaceId, string id, WorkspaceInvitationUpdateModel input)
    {
        input.WorkspaceId = workspaceId;
        input.Id = id;
        
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Invitation", "UpdateById").ExecuteAsync<WorkspaceInvitationModel>(input);
    }

    /// <summary>
    /// Deletes the record from the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public Task<WorkspaceInvitationModel> DeleteById(string workspaceId, string id)
    {
        return this.dataOps.Connect().QueryFirst("Workspace.Workspace.Invitation", "DeleteById").ExecuteAsync<WorkspaceInvitationModel>(new
        {
            WorkspaceId = workspaceId,
            Id = id
        });
    }
}
