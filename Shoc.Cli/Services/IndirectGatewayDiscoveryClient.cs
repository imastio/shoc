using System.Threading.Tasks;
using Imast.Ext.DiscoveryCore;
using Shoc.Cli.Model;

namespace Shoc.Cli.Services;

/// <summary>
/// The indirect gateway discovery
/// </summary>
public class IndirectGatewayDiscoveryClient : IDiscoveryClient
{
    /// <summary>
    /// The shoc profile
    /// </summary>
    private readonly ShocProfile profile;

    /// <summary>
    /// The endpoint service
    /// </summary>
    private readonly EndpointService endpointService;

    /// <summary>
    /// Creates new indirect gateway client
    /// </summary>
    /// <param name="profile">The shoc profile</param>
    /// <param name="endpointService"></param>
    public IndirectGatewayDiscoveryClient(ShocProfile profile, EndpointService endpointService)
    {
        this.profile = profile;
        this.endpointService = endpointService;
    }
    
    /// <summary>
    /// Gets the instance of the service
    /// </summary>
    /// <param name="service">The service name</param>
    /// <returns></returns>
    public async Task<DiscoveryInstance> GetInstance(string service)
    {
        var endpoints = await this.endpointService.GetEndpoints(profile);

        return await new GatewayDiscoveryClient(endpoints.Api).GetInstance(service);
    }

    /// <summary>
    /// Gets the next base url
    /// </summary>
    /// <param name="service">The service name</param>
    /// <returns></returns>
    public async Task<string> GetNextBaseUrl(string service)
    {
        var endpoints = await this.endpointService.GetEndpoints(profile);

        return await new GatewayDiscoveryClient(endpoints.Api).GetNextBaseUrl(service);
    }

    /// <summary>
    /// Gets the next api url
    /// </summary>
    /// <param name="service">The service name</param>
    /// <param name="api">The api name</param>
    /// <returns></returns>
    public async Task<string> GetApiUrl(string service, string api)
    {
        var endpoints = await this.endpointService.GetEndpoints(profile);

        return await new GatewayDiscoveryClient(endpoints.Api).GetApiUrl(service, api);
    }

    /// <summary>
    /// The gateway type
    /// </summary>
    public string Type => "indirect-gateway";
}