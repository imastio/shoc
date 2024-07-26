using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Yarp.ReverseProxy.Forwarder;
using System.Threading;
using Shoc.Core.Discovery;

namespace Shoc.Webgtw.Gateway;

/// <summary>
/// The custom request transformation
/// </summary>
public class RequestTransformer : HttpTransformer
{
    /// <summary>
    /// The discovery client
    /// </summary>
    private readonly IDiscoveryClient discoveryClient;

    /// <summary>
    /// Creates new instance of request transformer
    /// </summary>
    /// <param name="discoveryClient"></param>
    public RequestTransformer(IDiscoveryClient discoveryClient)
    {
        this.discoveryClient = discoveryClient;
    }

    /// <summary>
    /// A callback that is invoked prior to sending the proxied request. All HttpRequestMessage
    /// fields are initialized except RequestUri, which will be initialized after the
    /// callback if no value is provided. The string parameter represents the destination
    /// URI prefix that should be used when constructing the RequestUri. The headers
    /// are copied by the base implementation, excluding some protocol headers like HTTP/2
    /// pseudo headers (":authority").
    /// </summary>
    /// <param name="httpContext">The incoming request.</param>
    /// <param name="proxyRequest">The outgoing proxy request.</param>
    /// <param name="destinationPrefix">The uri prefix for the selected destination server which can be used to create the RequestUri.</param>
    /// <param name="cancelationToken">The cancellation token.</param>
    public override async ValueTask TransformRequestAsync(HttpContext httpContext, HttpRequestMessage proxyRequest, string destinationPrefix, CancellationToken cancelationToken)
    {
        // copy all request headers
        await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix, cancelationToken);

        // the service url
        var service = httpContext.Request.RouteValues["service"]?.ToString();

        // the API value
        var api = httpContext.Request.RouteValues["api"]?.ToString() ?? "/";

        // make sure starts with slash
        if (!api.StartsWith('/'))
        {
            api = $"/{api}";
        }
        
        // get the next url
        var nextUrl = await this.discoveryClient.GetApiBase(service);

        // keep original if no service is discovered
        if (string.IsNullOrWhiteSpace(nextUrl))
        {
            return;
        }
        
        // make a forwarding request URI
        proxyRequest.RequestUri = RequestUtilities.MakeDestinationAddress(nextUrl, api, httpContext.Request.QueryString);
        
        // suppress the original request header, use the one from the destination Uri.
        proxyRequest.Headers.Host = null;
    }
}



