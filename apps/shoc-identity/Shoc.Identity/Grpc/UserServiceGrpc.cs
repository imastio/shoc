using System.Threading.Tasks;
using Grpc.Core;
using Shoc.ApiCore.Access;
using Shoc.Core.OpenId;
using Shoc.Identity.Grpc.Users;
using Shoc.Identity.Model.User;
using Shoc.Identity.Services;

namespace Shoc.Identity.Grpc;

/// <summary>
/// The Grpc service implementation
/// </summary>
[BearerOnly]
public class UserServiceGrpc : Users.UserServiceGrpc.UserServiceGrpcBase
{
    /// <summary>
    /// The access authorization service
    /// </summary>
    private readonly IAccessAuthorization accessAuthorization;

    /// <summary>
    /// The user service
    /// </summary>
    private readonly UserService userService;

    /// <summary>
    /// Creates a new instance of the Grpc service
    /// </summary>
    /// <param name="accessAuthorization">The access authorization service</param>
    /// <param name="userService">The user service</param>
    public UserServiceGrpc(IAccessAuthorization accessAuthorization, UserService userService)
    {
        this.accessAuthorization = accessAuthorization;
        this.userService = userService;
    }
    
    /// <summary>
    /// Gets the user by id
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The request context</param>
    /// <returns></returns>
    public override async Task<GetUserResponse> GetById(GetUserByIdRequest request, ServerCallContext context)
    {
        // ensure authorization
        await this.accessAuthorization.RequireScopesAll(context.GetHttpContext(), new []{ KnownScopes.SVC });
        
        // get the resulting object
        var result = await this.userService.GetById(request.Id);

        // return the response
        return new GetUserResponse
        {
            User = Map(result)
        };
    }
    
    /// <summary>
    /// Gets the user by id
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="context">The request context</param>
    /// <returns></returns>
    public override async Task<GetUserResponse> GetByEmail(GetUserByEmailRequest request, ServerCallContext context)
    {
        // ensure authorization
        await this.accessAuthorization.CheckScopesAll(context.GetHttpContext(), new []{ KnownScopes.SVC });
        
        // get the resulting object
        var result = await this.userService.GetByEmail(request.Email);

        // return the response
        return new GetUserResponse
        {
            User = Map(result)
        };
    }

    /// <summary>
    /// Maps to the Grpc representation
    /// </summary>
    /// <param name="input">The input to map</param>
    /// <returns></returns>
    private static UserGrpcModel Map(UserModel input)
    {
        return new UserGrpcModel
        {
            Id = input.Id,
            Email = input.Email,
            EmailVerified = input.EmailVerified,
            Username = input.Username,
            Type = input.Type,
            UserState = input.UserState,
            FullName = input.FullName
        };
    }
}