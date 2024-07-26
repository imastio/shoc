using System;
using System.Threading.Tasks;
using Grpc.Core;
using Shoc.ApiCore.Auth;

namespace Shoc.ApiCore.GrpcClient;

/// <summary>
/// The Grpc client executor 
/// </summary>
public class GrpcClientExecutor<TClient>
{
    /// <summary>
    /// The instance to a client
    /// </summary>
    private readonly TClient client;

    /// <summary>
    /// The auth provider
    /// </summary>
    private readonly IAuthProvider authProvider;

    /// <summary>
    /// Creates new instance of Grpc Client executor
    /// </summary>
    /// <param name="client">The client to use</param>
    /// <param name="authProvider">The auth provider</param>
    public GrpcClientExecutor(TClient client, IAuthProvider authProvider)
    {
        this.client = client;
        this.authProvider = authProvider;
    }

    /// <summary>
    /// Execute the given grpc operation providing the secure authorized metadata
    /// </summary>
    /// <param name="securedFunction">The secured function</param>
    /// <param name="scopes">The set of scopes to authorize with</param>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <returns></returns>
    public Task<T> DoAuthorized<T>(Func<TClient, Metadata, Task<T>> securedFunction, string[] scopes)
    {
        return this.authProvider.DoAuthorized(token =>
        {
            // create headers with authorized context
            var headers = new Metadata 
            {
                { "Authorization", $"Bearer {token}" }
            };

            return securedFunction(this.client, headers);
        }, scopes);
    } 
    
    /// <summary>
    /// Execute the given grpc operation providing the secure authorized metadata
    /// </summary>
    /// <param name="securedFunction">The secured function</param>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <returns></returns>
    public Task<T> DoAuthorized<T>(Func<TClient, Metadata, Task<T>> securedFunction)
    {
        return this.DoAuthorized(securedFunction, []);
    } 
    
    /// <summary>
    /// Execute the given grpc operation providing the secure authorized metadata
    /// </summary>
    /// <param name="function">The secured function</param>
    /// <typeparam name="T">The type of the result</typeparam>
    /// <returns></returns>
    public Task<T> Do<T>(Func<TClient, Task<T>> function)
    {
        return function(this.client);
    } 
}