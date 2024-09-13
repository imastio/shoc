using System.Threading.Tasks;
using Grpc.Core;
using Shoc.ApiCore.Access;
using Shoc.Core.OpenId;
using Shoc.Workspace.Grpc.Workspaces;
using Shoc.Workspace.Model.Workspace;
using Shoc.Workspace.Services;

namespace Shoc.Workspace.Grpc;

/// <summary>
/// The Grpc service implementation
/// </summary>
[BearerOnly]
public class WorkspaceMembersServiceGrpc : WorkspaceMemberServiceGrpc.WorkspaceMemberServiceGrpcBase
{
    /// <summary>
    /// The access authorization service
    /// </summary>
    private readonly IAccessAuthorization accessAuthorization;

    /// <summary>
    /// The workspace service
    /// </summary>
    private readonly WorkspaceMemberService workspaceMemberService;

    /// <summary>
    /// Creates a new instance of the Grpc service
    /// </summary>
    /// <param name="accessAuthorization">The access authorization service</param>
    /// <param name="workspaceMemberService">The workspace member service</param>
    public WorkspaceMembersServiceGrpc(IAccessAuthorization accessAuthorization, WorkspaceMemberService workspaceMemberService)
    {
        this.accessAuthorization = accessAuthorization;
        this.workspaceMemberService = workspaceMemberService;
    }

    /// <summary>
    /// Gets the object by user id
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The context object</param>
    /// <returns></returns>
    public override async Task<GetWorkspaceMemberResponse> GetByUserId(GetWorkspaceMemberByUserIdRequest request, ServerCallContext context)
    {
        // ensure authorization
        await this.accessAuthorization.RequireScopesAll(context.GetHttpContext(), new []{ KnownScopes.SVC });
        
        // get the resulting object
        var result = await this.workspaceMemberService.GetByUserId(request.WorkspaceId, request.UserId);

        // return the response
        return new GetWorkspaceMemberResponse
        {
            Member = Map(result)
        };
    }
    
    /// <summary>
    /// Maps to the Grpc representation
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    private static WorkspaceMemberGrpcModel Map(WorkspaceMemberModel input)
    {
        return new WorkspaceMemberGrpcModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId,
            UserId = input.UserId,
            Role = input.Role
        };
    }
}