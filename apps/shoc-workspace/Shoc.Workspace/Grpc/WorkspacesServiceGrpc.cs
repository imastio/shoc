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
public class WorkspacesServiceGrpc : WorkspaceServiceGrpc.WorkspaceServiceGrpcBase
{
    /// <summary>
    /// The access authorization service
    /// </summary>
    private readonly IAccessAuthorization accessAuthorization;

    /// <summary>
    /// The workspace service
    /// </summary>
    private readonly WorkspaceService workspaceService;

    /// <summary>
    /// Creates a new instance of the Grpc service
    /// </summary>
    /// <param name="accessAuthorization">The access authorization service</param>
    /// <param name="workspaceService">The workspace service</param>
    public WorkspacesServiceGrpc(IAccessAuthorization accessAuthorization, WorkspaceService workspaceService)
    {
        this.accessAuthorization = accessAuthorization;
        this.workspaceService = workspaceService;
    }

    /// <summary>
    /// Gets the object by id
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The context object</param>
    /// <returns></returns>
    public override async Task<GetWorkspaceResponse> GetById(GetWorkspaceByIdRequest request, ServerCallContext context)
    {
        // ensure authorization
        await this.accessAuthorization.RequireScopesAll(context.GetHttpContext(), new []{ KnownScopes.SVC });
        
        // get the resulting object
        var result = await this.workspaceService.GetById(request.Id);

        // return the response
        return new GetWorkspaceResponse
        {
            Workspace = Map(result)
        };
    }

    /// <summary>
    /// Gets the object by name
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The context object</param>
    /// <returns></returns>
    public override async Task<GetWorkspaceResponse> GetByName(GetWorkspaceByNameRequest request, ServerCallContext context)
    {
        // ensure authorization
        await this.accessAuthorization.RequireScopesAll(context.GetHttpContext(), new []{ KnownScopes.SVC });
        
        // get the resulting object
        var result = await this.workspaceService.GetByName(request.Name);

        // return the response
        return new GetWorkspaceResponse
        {
            Workspace = Map(result)
        };
    }
    
    /// <summary>
    /// Maps to the Grpc representation
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    private static WorkspaceGrpcModel Map(WorkspaceModel input)
    {
        return new WorkspaceGrpcModel
        {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Status = input.Status,
            CreatedBy = input.CreatedBy
        };
    }
}