using System.Threading.Tasks;
using Grpc.Core;
using Shoc.ApiCore.Access;
using Shoc.Core.OpenId;
using Shoc.Registry.Grpc.Registries;
using Shoc.Registry.Services;

namespace Shoc.Registry.Grpc;

/// <summary>
/// The Grpc service implementation
/// </summary>
[BearerOnly]
public class RegistryServiceGrpc : Registries.RegistryServiceGrpc.RegistryServiceGrpcBase
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
    /// The registry grpc mapper
    /// </summary>
    private readonly RegistryGrpcMapper registryGrpcMapper;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="accessAuthorization">The access authorization</param>
    /// <param name="registryService">The registry service</param>
    /// <param name="registryGrpcMapper">The registry grpc mapper</param>
    public RegistryServiceGrpc(IAccessAuthorization accessAuthorization, RegistryService registryService, RegistryGrpcMapper registryGrpcMapper)
    {
        this.accessAuthorization = accessAuthorization;
        this.registryService = registryService;
        this.registryGrpcMapper = registryGrpcMapper;
    }

    /// <summary>
    /// Gets the registry by id
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The context object</param>
    /// <returns></returns>
    public override async Task<GetRegistryResponse> GetById(GetRegistryByIdRequest request, ServerCallContext context)
    {
        // ensure authorization
        await this.accessAuthorization.RequireScopesAll(context.GetHttpContext(), new []{ KnownScopes.SVC });

        // gets the system-wide default registry as default
        var registry = await this.registryService.GetByGlobalName("default");

        // build and return the result
        return new GetRegistryResponse
        {
            Registry = this.registryGrpcMapper.Map(registry)
        };
    }
}