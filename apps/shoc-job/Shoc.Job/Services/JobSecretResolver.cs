using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Job.Model;
using Shoc.Secret.Grpc.Secrets;

namespace Shoc.Job.Services;

/// <summary>
/// The secret resolver
/// </summary>
public class JobSecretResolver
{
    /// <summary>
    /// The grpc client provider
    /// </summary>
    private readonly IGrpcClientProvider grpcClientProvider;
    
    /// <summary>
    /// Creates new instance of the service
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public JobSecretResolver(IGrpcClientProvider grpcClientProvider)
    {
        this.grpcClientProvider = grpcClientProvider;
    }
    
    /// <summary>
    /// Gets unified secrets by names
    /// </summary>
    /// <param name="workspaceId">The workspace id</param>
    /// <param name="userId">The user id</param>
    /// <param name="names">The names</param>
    /// <returns></returns>
    public async Task<IEnumerable<UnifiedSecretGrpcModel>> ResolveByNames(string workspaceId, string userId, ICollection<string> names)
    {
        // no names to resolve
        if (names.Count == 0)
        {
            return Array.Empty<UnifiedSecretGrpcModel>();
        }

        // the resulting secrets
        IEnumerable<UnifiedSecretGrpcModel> results;
        
        // try getting object
        try {
            results = (await this.grpcClientProvider
                .Get<UnifiedSecretServiceGrpc.UnifiedSecretServiceGrpcClient>()
                .DoAuthorized(async (client, metadata) => await client.GetByNamesAsync(new GetUnifiedSecretsByNameRequest
                {
                    WorkspaceId = workspaceId,
                    UserId = userId,
                    Names = { names },
                    Plain = true
                }, metadata))).Secrets;
        }
        catch(Exception)
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_SECRETS, "Could not resolved referenced secrets").AsException();
        }

        return results;
    }
}