using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.GrpcClient;
using Shoc.Identity.Grpc.Users;

namespace Shoc.Workspace.Controllers;

/// <summary>
/// The echo controller
/// </summary>
[Route("api/echo")]
[ApiController]
[ShocExceptionHandler]
public class EchoController : ControllerBase
{
    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;
    
    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    public EchoController(IGrpcClientProvider grpcClientProvider)
    {
        this.grpcClientProvider = grpcClientProvider;
    }
    
    /// <summary>
    /// Gets the object of user
    /// </summary>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public Task<GetUserResponse> GetAll(string userId)
    {
        return this.grpcClientProvider
            .Get<UserServiceGrpc.UserServiceGrpcClient>()
            .DoAuthorized(async (client, metadata) => await client.GetByIdAsync(new GetUserByIdRequest { Id = userId }, metadata));
    }
}