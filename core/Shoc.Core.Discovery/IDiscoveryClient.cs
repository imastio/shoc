using System.Threading.Tasks;

namespace Shoc.Core.Discovery;

/// <summary>
/// The discovery client
/// </summary>
public interface IDiscoveryClient
{
    /// <summary>
    /// Gets the next base URL for the service or null
    /// </summary>
    /// <param name="service">The service</param>
    /// <returns></returns>
    Task<string> GetApiBase(string service);

    /// <summary>
    /// Gets the API url based on the service and api location or null
    /// </summary>
    /// <param name="service">The service</param>
    /// <param name="api">The api</param>
    /// <returns></returns>
    Task<string> GetApiUrl(string service, string api);
    
    /// <summary>
    /// Gets the gRPC url for the given service or null
    /// </summary>
    /// <param name="service">The service</param>
    /// <returns></returns>
    Task<string> GetGrpcUrl(string service);
}