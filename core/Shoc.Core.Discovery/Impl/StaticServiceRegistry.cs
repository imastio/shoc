using System.Threading.Tasks;

namespace Shoc.Core.Discovery.Impl;

/// <summary>
/// The static service registry
/// </summary>
public class StaticServiceRegistry : IServiceRegistry
{
    /// <summary>
    /// The instance of definitions
    /// </summary>
    protected readonly ServiceDefinitions definitions;
    
    /// <summary>
    /// Creates new instance of static service registry
    /// </summary>
    /// <param name="definitions">The definitions</param>
    public StaticServiceRegistry(ServiceDefinitions definitions)
    {
        this.definitions = definitions;
    }
    
    /// <summary>
    /// Gets the definition by the given service name or null if does not exist
    /// </summary>
    /// <param name="service">The service name</param>
    /// <returns></returns>
    public Task<ServiceDefinition> Get(string service)
    {
        // get the result
        var result =  this.definitions.Services.TryGetValue(service, out var definition) ? definition : null;

        return Task.FromResult(result);
    }
}