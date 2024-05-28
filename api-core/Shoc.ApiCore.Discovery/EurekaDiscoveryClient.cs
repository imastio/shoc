using System;
using System.Threading.Tasks;
using Imast.Ext.DiscoveryCore;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery.Eureka.AppInfo;

namespace Shoc.ApiCore.Discovery;

/// <summary>
/// The eureka based discovery client
/// </summary>
public class EurekaDiscoveryClient : IDiscoveryClient
{
    /// <summary>
    /// The type of discovery client
    /// </summary>
    public string Type => "eureka";

    /// <summary>
    /// The eureka client instance
    /// </summary>
    private readonly IEurekaClient eurekaClient;

    /// <summary>
    /// The fallback discovery client
    /// </summary>
    private readonly IDiscoveryClient fallback;

    /// <summary>
    /// Creates new instance of eureka discovery client
    /// </summary>
    /// <param name="eurekaClient">The eureka client</param>
    /// <param name="fallback">The fallback client</param>
    public EurekaDiscoveryClient(IEurekaClient eurekaClient, IDiscoveryClient fallback)
    {
        this.eurekaClient = eurekaClient;
        this.fallback = fallback;
    }

    /// <summary>
    /// Gets the instance of service 
    /// </summary>
    /// <param name="service">The name of service</param>
    /// <returns></returns>
    public async Task<DiscoveryInstance> GetInstance(string service)
    {
        // gets the next instance from eureka
        InstanceInfo instance;

        try
        {
            // try get value
            instance = await Task.Run(() => this.eurekaClient.GetNextServerFromEureka(service, false));
        }
        catch (Exception)
        {
            instance = null;
        }

        // build response based on received instance
        return instance switch
        {
            null when this.fallback != null => await this.fallback.GetInstance(service),
            null => null,
            _ => new DiscoveryInstance
            {
                Base = instance.HomePageUrl,
                Host = instance.HostName,
                Port = instance.Port
            }
        };
    }

    /// <summary>
    /// Gets the next base url
    /// </summary>
    /// <param name="service">The service</param>
    /// <returns></returns>
    public async Task<string> GetNextBaseUrl(string service)
    {
        // get the instance
        var instance = await this.GetInstance(service);

        // return base address
        return instance?.Base ?? $"http://{service}/";
    }

    /// <summary>
    /// Gets the api url from discovery
    /// </summary>
    /// <param name="service">The service name</param>
    /// <param name="api">The api url</param>
    /// <returns></returns>
    public async Task<string> GetApiUrl(string service, string api)
    {
        // get next base url
        var baseUrl = await this.GetNextBaseUrl(service);

        // indicate if slash divider exists
        var divider = baseUrl.EndsWith('/') || api.StartsWith('/') ? string.Empty : "/";

        // builds the final API url
        return $"{baseUrl}{divider}{api}";
    }
}
