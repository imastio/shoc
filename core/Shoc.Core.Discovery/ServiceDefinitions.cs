using System.Collections.Generic;

namespace Shoc.Core.Discovery;

/// <summary>
/// The service definitions
/// </summary>
public class ServiceDefinitions
{
    /// <summary>
    /// The registry of the services
    /// </summary>
    public IDictionary<string, ServiceDefinition> Services { get; set; }
}