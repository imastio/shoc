using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.DataProtection;
using Shoc.ApiCore.Access;
using Shoc.Core.OpenId;
using Shoc.Registry.Grpc.Registries;
using Shoc.Registry.Model.Credential;
using Shoc.Registry.Services;

namespace Shoc.Registry.Grpc;

/// <summary>
/// The Grpc service implementation
/// </summary>
[BearerOnly]
public class RegistryPlainCredentialServiceGrpc : Registries.RegistryPlainCredentialServiceGrpc.RegistryPlainCredentialServiceGrpcBase
{
    /// <summary>
    /// The access authorization service
    /// </summary>
    private readonly IAccessAuthorization accessAuthorization;

    /// <summary>
    /// The registry credential service
    /// </summary>
    private readonly RegistryCredentialService registryCredentialService;
    
    /// <summary>
    /// The credential protection provider
    /// </summary>
    private readonly CredentialProtectionProvider credentialProtectionProvider;

    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="accessAuthorization">The access authorizatoin</param>
    /// <param name="registryCredentialService">The registry credential service</param>
    /// <param name="credentialProtectionProvider">The credential protection provider</param>
    public RegistryPlainCredentialServiceGrpc(IAccessAuthorization accessAuthorization, RegistryCredentialService registryCredentialService, CredentialProtectionProvider credentialProtectionProvider)
    {
        this.accessAuthorization = accessAuthorization;
        this.registryCredentialService = registryCredentialService;
        this.credentialProtectionProvider = credentialProtectionProvider;
    }

    /// <summary>
    /// Gets the registry plain credentials for the workspace
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="context">The context</param>
    /// <returns></returns>
    public override async Task<GetRegistryPlainCredentialsResponse> GetByWorkspace(GetRegistryPlainCredentialsRequest request, ServerCallContext context)
    {
        // ensure authorization
        await this.accessAuthorization.RequireScopesAll(context.GetHttpContext(), new []{ KnownScopes.SVC });

        // get workspace-level credentials
        var workspaceCredentials = await this.registryCredentialService.GetBy(request.RegistryId, new RegistryCredentialFilter
        {
            ByWorkspace = true,
            WorkspaceId = request.WorkspaceId,
            ByUser = true,
            UserId = null
        });
        
        // get user-level credentials
        var userCredentials = await this.registryCredentialService.GetBy(request.RegistryId, new RegistryCredentialFilter
        {
            ByWorkspace = true,
            WorkspaceId = request.WorkspaceId,
            ByUser = true,
            UserId = request.UserId
        });

        // all credentials
        var all = workspaceCredentials.Union(userCredentials);

        // create a protector
        var protector = this.credentialProtectionProvider.Create();

        return new GetRegistryPlainCredentialsResponse
        {
            Credentials = { all.Select(credential => Map(credential, protector)) }
        };

    }

    /// <summary>
    /// Maps to the grpc representation
    /// </summary>
    /// <param name="credential">The credential to map</param>
    /// <param name="protector">The protector</param>
    /// <returns></returns>
    private static RegistryPlainCredentialGrpcModel Map(RegistryCredentialModel credential, IDataProtector protector)
    {
        return new RegistryPlainCredentialGrpcModel
        {
            Id = credential.Id,
            RegistryId = credential.RegistryId,
            WorkspaceId = credential.WorkspaceId ?? string.Empty,
            UserId = credential.UserId ?? string.Empty,
            Source = credential.Source,
            Username = credential.Username,
            PasswordPlain = protector.Unprotect(credential.PasswordEncrypted),
            Email = credential.Email ?? string.Empty,
            PushAllowed = credential.PushAllowed,
            PullAllowed = credential.PullAllowed
        };
    }
}