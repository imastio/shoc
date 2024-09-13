namespace Shoc.ApiCore.GrpcClient;

/// <summary>
/// The Grpc client provider interface
/// </summary>
public interface IGrpcClientProvider
{
    /// <summary>
    /// Gets the grpc client executor for the given type
    /// </summary>
    /// <typeparam name="TClient">The client type</typeparam>
    /// <returns></returns>
    GrpcClientExecutor<TClient> Get<TClient>();
}