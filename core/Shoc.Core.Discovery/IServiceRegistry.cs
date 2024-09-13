using System.Threading.Tasks;

namespace Shoc.Core.Discovery;

/// <summary>
/// The service registry
/// </summary>
public interface IServiceRegistry
{
    /// <summary>
    /// Gets the service definition by the name
    /// </summary>
    /// <param name="service">The service identifier</param>
    /// <returns></returns>
    Task<ServiceDefinition> Get(string service);
}