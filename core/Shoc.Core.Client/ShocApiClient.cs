using System.Net.Http;
using Imast.Ext.ApiClient;
using Imast.Ext.DiscoveryCore;

namespace Shoc.Core.Client;

/// <summary>
/// The  api client
/// </summary>
public class ShocApiClient : ApiClientBase
{
    /// <summary>
    /// Creates new instance of pay client
    /// </summary>
    /// <param name="client">The client name</param>
    /// <param name="service">The service</param>
    /// <param name="discovery">The discovery</param>
    public ShocApiClient(string client, string service, IDiscoveryClient discovery) : base(client, service,
        discovery)
    {
    }

    /// <summary>
    /// Builds the http message 
    /// </summary>
    /// <param name="method">The method</param>
    /// <param name="url">The url</param>
    /// <param name="input">The input to add</param>
    /// <param name="headers">The headers</param>
    /// <returns></returns>
    public static HttpRequestMessage BuildMessage(HttpMethod method, string url, object input,
        params HeaderSpec[] headers)
    {
        // build the message
        var message = new HttpRequestMessage(method, url);

        // add input params
        if (input != null)
        {
            message.Content = AsJson(input);
        }

        // add all headers
        foreach (var header in headers)
        {
            message.Headers.Add(header.Header, new[] {header.Value});
        }

        // return message
        return message;
    }
}
