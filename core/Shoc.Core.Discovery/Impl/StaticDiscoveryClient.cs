using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shoc.Core.Discovery.Impl;

/// <summary>
/// The static discovery client implementation
/// </summary>
public class StaticDiscoveryClient : IDiscoveryClient
{
    /// <summary>
    /// The service registry
    /// </summary>
    protected readonly IServiceRegistry serviceRegistry;

    /// <summary>
    /// The static discovery options
    /// </summary>
    protected readonly StaticDiscoveryOptions options;

    /// <summary>
    /// The static discovery client
    /// </summary>
    /// <param name="serviceRegistry">The service registry instance</param>
    /// <param name="options">The static discovery options</param>
    public StaticDiscoveryClient(IServiceRegistry serviceRegistry, StaticDiscoveryOptions options)
    {
        this.serviceRegistry = serviceRegistry;
        this.options = options;
    }

    /// <summary>
    /// Gets the next base URL for the service or null
    /// </summary>
    /// <param name="service">The service</param>
    /// <returns></returns>
    public async Task<string> GetApiBase(string service)
    {
        // get the definition
        var definition = await this.serviceRegistry.Get(service);

        // make sure definition exists
        if (definition == null)
        {
            return null;
        }
        
        // choose the protocol from options or HTTP otherwise
        var protocol = string.IsNullOrWhiteSpace(this.options.DefaultProtocol) ? ServiceProtocolTypes.HTTP : this.options.DefaultProtocol.ToLowerInvariant();

        // check if protocol is not expected for the API 
        if (protocol != ServiceProtocolTypes.HTTP && protocol != ServiceProtocolTypes.HTTPS)
        {
            return null;
        }
        
        // try finding port definition for the given protocol and the API role
        var portDefinition = definition.Ports.FirstOrDefault(portDef => protocol.Equals(portDef.Protocol) && portDef.Roles.Contains(ServicePortRoles.API));

        // no such port
        if (portDefinition == null)
        {
            return null;
        }

        // get the host with the given service and definition
        var host = this.GetHostname(service, definition);

        // if host is there build the base URL
        return host == null ? null : $"{protocol}://{host}:{portDefinition.Port}";
    }

    /// <summary>
    /// Gets the API url based on the service and api location or null
    /// </summary>
    /// <param name="service">The service</param>
    /// <param name="api">The api</param>
    /// <returns></returns>
    public async Task<string> GetApiUrl(string service, string api)
    {
        // gets the base URL
        var baseUrl = await this.GetApiBase(service);

        // no API if no base URL
        if (baseUrl == null || api == null)
        {
            return null;
        }
        
        // remove last slash from base url
        baseUrl = baseUrl.TrimEnd('/');
        
        // remove first slash from api
        api = api.TrimStart('/');

        // build the API url
        return $"{baseUrl}/{api}";
    }

    /// <summary>
    /// Gets the gRPC url for the given service or null
    /// </summary>
    /// <param name="service">The service</param>
    /// <returns></returns>
    public async Task<string> GetGrpcUrl(string service)
    {
        // get the definition
        var definition = await this.serviceRegistry.Get(service);

        // make sure definition exists
        if (definition == null)
        {
            return null;
        }
        
        // choose the protocol from options or HTTP otherwise
        var protocol = string.IsNullOrWhiteSpace(this.options.DefaultGrpcProtocol) ? ServiceProtocolTypes.HTTP : this.options.DefaultGrpcProtocol.ToLowerInvariant();

        // check if protocol is not expected for the API 
        if (protocol != ServiceProtocolTypes.HTTP && protocol != ServiceProtocolTypes.HTTPS)
        {
            return null;
        }
        
        // try finding port definition for the given protocol and the API role
        var portDefinition = definition.Ports.FirstOrDefault(portDef => protocol.Equals(portDef.Protocol) && portDef.Roles.Contains(ServicePortRoles.GRPC));

        // no such port
        if (portDefinition == null)
        {
            return null;
        }

        // get the host with the given service and definition
        var host = this.GetHostname(service, definition);

        // if host is there build the base URL
        return host == null ? null : $"{protocol}://{host}:{portDefinition.Port}";
    }
    
    /// <summary>
    /// Gets the hostname for the definition based on the given options
    /// </summary>
    /// <param name="service">The service to resolve</param>
    /// <param name="definition">The definition</param>
    /// <returns></returns>
    private string GetHostname(string service, ServiceDefinition definition)
    {
        // use service name as a host by default
        return this.options.HostingType switch
        {
            HostingTypes.K8S => GetKubernetesHostname(service, definition),
            HostingTypes.LOCALHOST => "localhost",
            HostingTypes.DOCKER => service,
            _ => service
        };
    }

    /// <summary>
    /// Gets the hostname for the Kubernetes cluster
    /// </summary>
    /// <param name="service">The target service</param>
    /// <param name="definition">The definition</param>
    /// <returns></returns>
    private static string GetKubernetesHostname(string service, ServiceDefinition definition)
    {
        // in case of missing or default namespace use the service name itself
        if (string.IsNullOrEmpty(definition.Namespace) || definition.Namespace.Equals("default", StringComparison.InvariantCulture))
        {
            return service;
        }

        return $"{service}.{definition.Namespace}";
    }
}