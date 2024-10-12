using System.Threading.Tasks;
using Grpc.Core;
using Shoc.ApiCore.Access;
using Shoc.Core.OpenId;
using Shoc.Registry.Grpc.Registries;
using Shoc.Registry.Model.Registry;
using Shoc.Registry.Services;

namespace Shoc.Registry.Grpc;

/// <summary>
/// The Grpc service implementation
/// </summary>
[BearerOnly]
public class WorkspaceDefaultRegistryServiceGrpc : Registries.WorkspaceDefaultRegistryServiceGrpc.WorkspaceDefaultRegistryServiceGrpcBase
{
    /// <summary>
    /// The access authorization service
    /// </summary>
    private readonly IAccessAuthorization accessAuthorization;

    /// <summary>
    /// The registry service
    /// </summary>
    private readonly RegistryService registryService;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="accessAuthorization">The access authorization</param>
    /// <param name="registryService">The registry service</param>
    public WorkspaceDefaultRegistryServiceGrpc(IAccessAuthorization accessAuthorization, RegistryService registryService)
    {
        this.accessAuthorization = accessAuthorization;
        this.registryService = registryService;
    }

    /// <summary>
    /// Gets the workspace default registry
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The context object</param>
    /// <returns></returns>
    public override async Task<GetWorkspaceDefaultRegistryResponse> GetByWorkspaceId(GetWorkspaceDefaultRegistryRequest request, ServerCallContext context)
    {
        // ensure authorization
        await this.accessAuthorization.RequireScopesAll(context.GetHttpContext(), new []{ KnownScopes.SVC });

        // gets the system-wide default registry as default
        var registry = await this.registryService.GetByGlobalName("default");

        // build and return the result
        return new GetWorkspaceDefaultRegistryResponse
        {
            Registry = Map(registry)
        };
    }
    
    /// <summary>
    /// Maps to the Grpc representation
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    private static RegistryGrpcModel Map(RegistryModel input)
    {
        return new RegistryGrpcModel
        {
            Id = input.Id,
            WorkspaceId = input.WorkspaceId ?? string.Empty,
            Name = input.Name,
            DisplayName = input.DisplayName,
            Status = input.Status,
            Provider = input.Provider,
            UsageScope = input.UsageScope,
            Registry = input.Registry,
            Namespace = input.Name
        };
    }
}