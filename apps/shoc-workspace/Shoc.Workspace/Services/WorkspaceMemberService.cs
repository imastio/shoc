using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Workspace.Data;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Services;

/// <summary>
/// The workspace member service
/// </summary>
public class WorkspaceMemberService : WorkspaceServiceBase
{
    /// <summary>
    /// The workspace member repository
    /// </summary>
    private readonly IWorkspaceMemberRepository workspaceMemberRepository;
    
    /// <summary>
    /// Creates new instance of member workspace
    /// </summary>
    /// <param name="workspaceMemberRepository">The workspace member repository</param>
    /// <param name="workspaceRepository">The workspace repository</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public WorkspaceMemberService(IWorkspaceMemberRepository workspaceMemberRepository, IWorkspaceRepository workspaceRepository, IGrpcClientProvider grpcClientProvider) : base(workspaceRepository, grpcClientProvider)
    {
        this.workspaceMemberRepository = workspaceMemberRepository;
    }

    /// <summary>
    /// Gets all the members of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public async Task<IEnumerable<WorkspaceMemberModel>> GetAll(string workspaceId)
    {
        // require workspace to exist
        await this.RequireWorkspaceById(workspaceId);

        // load from the database
        return await this.workspaceMemberRepository.GetAll(workspaceId);
    }

    /// <summary>
    /// Gets all the extended members of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public async Task<IEnumerable<WorkspaceMemberExtendedModel>> GetAllExtended(string workspaceId)
    {
        // require workspace to exist
        await this.RequireWorkspaceById(workspaceId);

        // load from the database
        return await this.workspaceMemberRepository.GetAllExtended(workspaceId);
    }

    /// <summary>
    /// Gets the particular membership record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The membership id</param>
    /// <returns></returns>
    public async Task<WorkspaceMemberModel> GetById(string workspaceId, string id)
    {
        // make sure workspace exists
        await this.RequireWorkspaceById(workspaceId);

        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by id
        var result = await this.workspaceMemberRepository.GetById(workspaceId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }

    /// <summary>
    /// Gets the particular extended membership record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The membership id</param>
    /// <returns></returns>
    public async Task<WorkspaceMemberExtendedModel> GetExtendedById(string workspaceId, string id)
    {
        // make sure workspace exists
        await this.RequireWorkspaceById(workspaceId);

        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by id
        var result = await this.workspaceMemberRepository.GetExtendedById(workspaceId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }
    
    /// <summary>
    /// Gets the particular membership record in the workspace by user id
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public async Task<WorkspaceMemberModel> GetByUserId(string workspaceId, string userId)
    {
        // make sure workspace exists
        await this.RequireWorkspaceById(workspaceId);

        // user id should be a valid string
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by user id
        var result = await this.workspaceMemberRepository.GetByUserId(workspaceId, userId);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }

    /// <summary>
    /// Creates new membership in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The workspace membership create input</param>
    /// <returns></returns>
    public async Task<WorkspaceMemberModel> Create(string workspaceId, WorkspaceMemberCreateModel input)
    {
        // refer to correct object
        input.WorkspaceId = workspaceId;
        
        // ensure workspace exists
        await this.RequireWorkspaceById(workspaceId);

        // ensure user exists
        await this.RequireUser(input.UserId);
        
        // ensure role is valid
        ValidateRole(input.Role);

        // owner cannot be added as a member
        if (input.Role == WorkspaceRoles.OWNER)
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_ROLE).AsException();
        }
         
        // try get existing membership record for the user
        var existing = await this.workspaceMemberRepository.GetByUserId(workspaceId, input.UserId);

        // raise error if user is already member
        if (existing != null)
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.ALREADY_MEMBER).AsException();
        }

        // perform the operation
        return await this.workspaceMemberRepository.Create(input);
    }

    /// <summary>
    /// Update the existing membership record
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The workspace membership update input</param>
    /// <returns></returns>
    public async Task<WorkspaceMemberModel> UpdateById(string workspaceId, string id, WorkspaceMemberUpdateModel input)
    {
        // ensure to refer to correct object
        input.WorkspaceId = workspaceId;
        input.Id = id;

        // ensure workspace exists
        await this.RequireWorkspaceById(workspaceId);
        
        // validate role
        ValidateRole(input.Role);
        
        // owner cannot be added as a member
        if (input.Role == WorkspaceRoles.OWNER)
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_ROLE).AsException();
        }

        // try get existing object
        var existing = await this.GetById(workspaceId, id);

        // if modifying owner record we cannot downgrade it
        if (existing.Role == WorkspaceRoles.OWNER && input.Role != WorkspaceRoles.OWNER)
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_ROLE).AsException();
        }
        
        // perform the operation
        return await this.workspaceMemberRepository.UpdateById(workspaceId, id, input);
    }

    /// <summary>
    /// Deletes the member user from the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public async Task<WorkspaceMemberModel> DeleteById(string workspaceId, string id)
    {
        // make sure record exists
        var existing = await this.GetById(workspaceId, id);

        // you cannot remove the owner
        if (existing.Role == WorkspaceRoles.OWNER)
        {
            throw ErrorDefinition.Validation().AsException();
        }
        
        // perform the operation
        var deleted = await this.workspaceMemberRepository.DeleteById(workspaceId, id);

        // no such object 
        if (deleted == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return existing object
        return existing;
    }

    /// <summary>
    /// Validates the given role
    /// </summary>
    /// <param name="role">The role to validate</param>
    private static void ValidateRole(string role)
    {
        // ensure role is defined
        if (!WorkspaceRoles.ALL.Contains(role))
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_ROLE).AsException();
        }
    }
}