namespace Shoc.Core.Discovery;

/// <summary>
/// The static discovery options
/// </summary>
public class StaticDiscoveryOptions
{
    /// <summary>
    /// The hosting type
    /// </summary>
    public string HostingType { get; set; }
    
    /// <summary>
    /// The default protocol
    /// </summary>
    public string DefaultProtocol { get; set; }
    
    /// <summary>
    /// The default grpc protocol
    /// </summary>
    public string DefaultGrpcProtocol { get; set; }
}