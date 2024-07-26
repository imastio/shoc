using System.Collections.Generic;

namespace Shoc.Core.Discovery;

/// <summary>
/// The service definition
/// </summary>
public class ServiceDefinition
{
    /// <summary>
    /// The service namespace
    /// </summary>
    public string Namespace { get; set; }
    
    /// <summary>
    /// The discovered host
    /// </summary>
    public IEnumerable<ServicePortDefinition> Ports { get; set; }
    
    /// <summary>
    /// The service is externally available
    /// </summary>
    public bool ExternalAvailability { get; set; }
    
    /// <summary>
    /// The http port if available
    /// </summary>
    public int? HttpPort { get; set; }
    
    /// <summary>
    /// The https port if available
    /// </summary>
    public int? HttpsPort { get; set; }
    
    /// <summary>
    /// The Grpc http port
    /// </summary>
    public int? GrpcHttpPort { get; set; }
    
    /// <summary>
    /// The Grpc https port
    /// </summary>
    public int? GrpcHttpsPort { get; set; }
}