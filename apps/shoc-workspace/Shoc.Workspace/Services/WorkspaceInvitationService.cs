using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Workspace.Data;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Services;

/// <summary>
/// The workspace invitation service
/// </summary>
public class WorkspaceInvitationService : WorkspaceServiceBase
{
    /// <summary>
    /// Invitation expires in 30 days by default
    /// </summary>
    private const int INVITE_EXPIRATION_DAYS = 30;
    
    /// <summary>
    /// Roles allowed for invitation
    /// </summary>
    private static readonly ISet<string> ALLOWED_ROLES = new HashSet<string>
    {
        WorkspaceRoles.MEMBER,
        WorkspaceRoles.GUEST
    };

    /// <summary>
    /// The workspace invitation repository
    /// </summary>
    private readonly IWorkspaceInvitationRepository workspaceInvitationRepository;

    /// <summary>
    /// The workspace member repository
    /// </summary>
    private readonly IWorkspaceMemberRepository workspaceMemberRepository;
    
    /// <summary>
    /// Creates new instance of member workspace
    /// </summary>
    /// <param name="workspaceInvitationRepository">The workspace invitation repository</param>
    /// <param name="workspaceMemberRepository">The workspace member repository</param>
    /// <param name="workspaceRepository">The workspace repository</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public WorkspaceInvitationService(IWorkspaceInvitationRepository workspaceInvitationRepository, IWorkspaceMemberRepository workspaceMemberRepository, IWorkspaceRepository workspaceRepository, IGrpcClientProvider grpcClientProvider) : base(workspaceRepository, grpcClientProvider)
    {
        this.workspaceInvitationRepository = workspaceInvitationRepository;
        this.workspaceMemberRepository = workspaceMemberRepository;
    }

    /// <summary>
    /// Gets all the invitations of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public async Task<IEnumerable<WorkspaceInvitationModel>> GetAll(string workspaceId)
    {
        // require workspace to exist
        await this.RequireWorkspaceById(workspaceId);

        // load from the database
        return await this.workspaceInvitationRepository.GetAll(workspaceId);
    }

    /// <summary>
    /// Gets all the extended invitations of the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <returns></returns>
    public async Task<IEnumerable<WorkspaceInvitationExtendedModel>> GetAllExtended(string workspaceId)
    {
        // require workspace to exist
        await this.RequireWorkspaceById(workspaceId);

        // load from the database
        return await this.workspaceInvitationRepository.GetAllExtended(workspaceId);
    }

    /// <summary>
    /// Gets the particular invitation record in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The invitation id</param>
    /// <returns></returns>
    public async Task<WorkspaceInvitationModel> GetById(string workspaceId, string id)
    {
        // make sure workspace exists
        await this.RequireWorkspaceById(workspaceId);
        
        // id should be a valid string
        if (string.IsNullOrWhiteSpace(id))
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // try getting object by id
        var result = await this.workspaceInvitationRepository.GetById(workspaceId, id);

        // make sure object exists
        if (result == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return the object
        return result;
    }

    /// <summary>
    /// Creates new invitation in the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="input">The workspace invitation create input</param>
    /// <returns></returns>
    public async Task<WorkspaceInvitationModel> Create(string workspaceId, WorkspaceInvitationCreateModel input)
    {
        // refer to correct object
        input.WorkspaceId = workspaceId;
        
        // validate email
        ValidateEmail(input.Email);
        
        // validate role
        ValidateInvitationRole(input.Role);
        
        // ensure workspace exists
        await this.RequireWorkspaceById(workspaceId);

        // try to get existing invitation for the given email
        var existing = await this.workspaceInvitationRepository.GetByEmail(workspaceId, input.Email);

        // there is an invitation
        if (existing != null)
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.ALREADY_INVITED).AsException();
        }
        
        // build default invitation expiration
        input.Expiration = DateTime.UtcNow.AddDays(INVITE_EXPIRATION_DAYS);

        // try to get member by the email we try to invite
        var member = await this.workspaceMemberRepository.GetByEmail(workspaceId, input.Email);

        // check if already a member
        if (member != null)
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.ALREADY_MEMBER).AsException();
        }
        
        // validate inviting party
        await this.RequireUser(input.InvitedBy);
        
        // perform the operation
        return await this.workspaceInvitationRepository.Create(input);
    }

    /// <summary>
    /// Update the existing invitation record
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <param name="input">The workspace invitation update input</param>
    /// <returns></returns>
    public async Task<WorkspaceInvitationModel> UpdateById(string workspaceId, string id, WorkspaceInvitationUpdateModel input)
    {
        // ensure to refer to correct object
        input.WorkspaceId = workspaceId;
        input.Id = id;
        
        // validate role
        ValidateInvitationRole(input.Role);
        
        // try get existing object to validate it exists
        await this.GetById(workspaceId, id);

        // build default invitation expiration
        input.Expiration = DateTime.UtcNow.AddDays(INVITE_EXPIRATION_DAYS);
        
        // perform the operation
        return await this.workspaceInvitationRepository.UpdateById(workspaceId, id, input);
    }

    /// <summary>
    /// Deletes the invitation user from the workspace
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="id">The record id</param>
    /// <returns></returns>
    public async Task<WorkspaceInvitationModel> DeleteById(string workspaceId, string id)
    {
        // make sure record exists
        var existing = await this.GetById(workspaceId, id);
        
        // perform the operation
        var deleted = await this.workspaceInvitationRepository.DeleteById(workspaceId, id);

        // no such object 
        if (deleted == null)
        {
            throw ErrorDefinition.NotFound().AsException();
        }

        // return existing object
        return existing;
    }

    /// <summary>
    /// Validate the invitation role
    /// </summary>
    /// <param name="role">The role to validate</param>
    private static void ValidateInvitationRole(string role)
    {
        // make sure role is allowed
        if (!ALLOWED_ROLES.Contains(role))
        {
            throw ErrorDefinition.Validation(WorkspaceErrors.INVALID_ROLE).AsException();
        }
    }
}