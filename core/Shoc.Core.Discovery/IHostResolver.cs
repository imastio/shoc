using System.Threading.Tasks;

namespace Shoc.Core.Discovery;

/// <summary>
/// The host resolver interface
/// </summary>
public interface IHostResolver
{
    /// <summary>
    /// Resolves the hostname for the given service
    /// </summary>
    /// <param name="service">The service</param>
    /// <returns></returns>
    Task<string> Resolve(string service);
}