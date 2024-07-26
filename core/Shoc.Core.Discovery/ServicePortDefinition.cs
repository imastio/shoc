using System.Collections.Generic;

namespace Shoc.Core.Discovery;

/// <summary>
/// The service port definition
/// </summary>
public class ServicePortDefinition
{
    /// <summary>
    /// The protocol name
    /// </summary>
    public string Protocol { get; set; }
    
    /// <summary>
    /// The set of roles assigned
    /// </summary>
    public IEnumerable<string> Roles { get; set; }
    
    /// <summary>
    /// The port number
    /// </summary>
    public int Port { get; set; }
}