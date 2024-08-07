using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Identity.Grpc.Users;
using Shoc.Workspace.Data;
using Shoc.Workspace.Model;
using Shoc.Workspace.Model.User;
using Shoc.Workspace.Model.Workspace;

namespace Shoc.Workspace.Services;

/// <summary>
/// The user invitation service
/// </summary>
public class UserInvitationService
{
    /// <summary>
    /// The workspace invitation service
    /// </summary>
    private readonly WorkspaceInvitationService workspaceInvitationService;

    /// <summary>
    /// The workspace member service
    /// </summary>
    private readonly WorkspaceMemberService workspaceMemberService;

    /// <summary>
    /// The workspace invitation repository
    /// </summary>
    private readonly IWorkspaceInvitationRepository workspaceInvitationRepository;

    /// <summary>
    /// The user invitation repository
    /// </summary>
    private readonly IUserInvitationRepository userInvitationRepository;
    
    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="workspaceInvitationService">The workspace invitation service</param>
    /// <param name="workspaceMemberService">The workspace member service</param>
    /// <param name="workspaceInvitationRepository">The workspace invitation repository</param>
    /// <param name="userInvitationRepository">The user invitation repository</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public UserInvitationService(WorkspaceInvitationService workspaceInvitationService, WorkspaceMemberService workspaceMemberService, IWorkspaceInvitationRepository workspaceInvitationRepository, IUserInvitationRepository userInvitationRepository, IGrpcClientProvider grpcClientProvider)
    {
        this.workspaceInvitationService = workspaceInvitationService;
        this.workspaceMemberService = workspaceMemberService;
        this.workspaceInvitationRepository = workspaceInvitationRepository;
        this.userInvitationRepository = userInvitationRepository;
        this.grpcClientProvider = grpcClientProvider;
    }

    /// <summary>
    /// Gets all the user invitations 
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public Task<IEnumerable<UserInvitationModel>> GetAll(string userId)
    {
        return this.userInvitationRepository.GetAll(userId);
    }

    /// <summary>
    /// Counts all the user invitations 
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns></returns>
    public async Task<UserInvitationsCountModel> CountAll(string userId)
    {
        // count objects
        var count = await this.userInvitationRepository.CountAll(userId);

        return new UserInvitationsCountModel
        {
            Count = count
        };
    }
    /// <summary>
    /// Accept the user invitation by the current user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="input">The accept input</param>
    /// <returns></returns>
    public async Task<UserInvitationAcceptedModel> Accept(string userId, UserInvitationAcceptModel input)
    {
        // load the invitation
        var invitation = await this.workspaceInvitationService.GetById(input.WorkspaceId, input.Id);
        
        // if invitation already expired raise an error
        if (invitation.Expiration < DateTime.UtcNow)
        {
            // delete the invitation
            await this.workspaceInvitationRepository.DeleteById(input.WorkspaceId, input.Id);

            // raise the error
            throw ErrorDefinition.Validation(WorkspaceErrors.EXPIRED_INVITATION).AsException();
        }
        
        // try getting object
        var userResponse = await this.grpcClientProvider
            .Get<UserServiceGrpc.UserServiceGrpcClient>()
            .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetUserByIdRequest{Id = userId}, metadata));
        
        // if invited user email does not match current user email raise an error
        if (!string.Equals(userResponse.User.Email, invitation.Email))
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // create the membership record
        var created = await this.workspaceMemberService.Create(input.WorkspaceId, new WorkspaceMemberCreateModel
        {
            WorkspaceId = invitation.WorkspaceId,
            UserId = userId,
            Role = invitation.Role
        });

        // delete the invitation on success
        await this.workspaceInvitationRepository.DeleteById(input.WorkspaceId, input.Id);
        
        // accepted result
        return new UserInvitationAcceptedModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            MembershipId = created.Id
        };
    }
    
    /// <summary>
    /// Decline the user invitation by the current user
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="input">The decline input</param>
    /// <returns></returns>
    public async Task<UserInvitationDeclinedModel> Decline(string userId, UserInvitationDeclineModel input)
    {
        // get the invitation
        var invitation = await workspaceInvitationService.GetById(input.WorkspaceId, input.Id);
        
        // try getting object
        var userResponse = await this.grpcClientProvider
            .Get<UserServiceGrpc.UserServiceGrpcClient>()
            .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetUserByIdRequest{Id = userId}, metadata));
        
        // if invited user email does not match current user email raise an error
        if (!string.Equals(userResponse.User.Email, invitation.Email))
        {
            throw ErrorDefinition.NotFound().AsException();
        }
        
        // delete the invitation
        var deleted = await this.workspaceInvitationService.DeleteById(input.WorkspaceId, input.Id);
        
        // declined result
        return new UserInvitationDeclinedModel
        {
            Id = deleted.Id,
            WorkspaceId = deleted.WorkspaceId
        };
    }
}