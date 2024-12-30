using System.Threading.Tasks;
using Grpc.Core;
using Shoc.ApiCore.Access;
using Shoc.Core.OpenId;
using Shoc.Package.Grpc.Packages;
using Shoc.Package.Services;

namespace Shoc.Package.Grpc;

/// <summary>
/// The grpc service implementation
/// </summary>
[BearerOnly]
public class PackageServiceGrpc : Packages.PackageServiceGrpc.PackageServiceGrpcBase
{
    /// <summary>
    /// The access authorization service
    /// </summary>
    private readonly IAccessAuthorization accessAuthorization;

    /// <summary>
    /// The package service
    /// </summary>
    private readonly PackageService packageService;

    /// <summary>
    /// The mapper instance
    /// </summary>
    private readonly PackageGrpcMapper mapper;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="accessAuthorization">The access authorization</param>
    /// <param name="packageService">The package service</param>
    /// <param name="mapper">The mapper instance</param>
    public PackageServiceGrpc(IAccessAuthorization accessAuthorization, PackageService packageService, PackageGrpcMapper mapper)
    {
        this.accessAuthorization = accessAuthorization;
        this.packageService = packageService;
        this.mapper = mapper;
    }

    /// <summary>
    /// Gets the package by id
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The context object</param>
    /// <returns></returns>
    public override async Task<GetPackageResponse> GetById(GetPackageByIdRequest request, ServerCallContext context)
    {
        // ensure authorization
        await this.accessAuthorization.RequireScopesAll(context.GetHttpContext(), new []{ KnownScopes.SVC });
        
        // gets the object by id
        var result = await this.packageService.GetExtendedById(request.WorkspaceId, request.Id);

        // build and return the result
        return new GetPackageResponse
        {
            Package = this.mapper.Map(result)
        };
    }
}