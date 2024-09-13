using System;
using Microsoft.Extensions.DependencyInjection;
using Shoc.ApiCore.Auth;

namespace Shoc.ApiCore.GrpcClient;

/// <summary>
/// The Grpc client provider implementation
/// </summary>
public class GrpcClientProvider : IGrpcClientProvider
{
    /// <summary>
    /// The service provider
    /// </summary>
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// The authentication provider
    /// </summary>
    private readonly IAuthProvider authProvider;

    /// <summary>
    /// Creates new instance of Grpc client provider
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <param name="authProvider">The auth provider</param>
    public GrpcClientProvider(IServiceProvider serviceProvider, IAuthProvider authProvider)
    {
        this.serviceProvider = serviceProvider;
        this.authProvider = authProvider;
    }

    /// <summary>
    /// Gets the grpc client executor for the given type
    /// </summary>
    /// <typeparam name="TClient">The client type</typeparam>
    /// <returns></returns>
    public GrpcClientExecutor<TClient> Get<TClient>()
    {
        // gets the required service from the collection
        var client = this.serviceProvider.GetRequiredService<TClient>();

        return new GrpcClientExecutor<TClient>(client, this.authProvider);
    }
}