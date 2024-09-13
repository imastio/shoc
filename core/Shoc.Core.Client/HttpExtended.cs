using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Shoc.Core.Client;

/// <summary>
/// The extensions to HttpClient
/// </summary>
public static class HttpExtended
{
    /// <summary>
    /// Ensure that message is received with success
    /// </summary>
    /// <param name="message">The message</param>
    /// <returns></returns>
    public static HttpResponseMessage EnsureShocSuccess(this HttpResponseMessage message)
    {
        // everything is fine
        if (message.IsSuccessStatusCode)
        {
            return message;
        }

        // a known error
        if (message.Headers.Contains("X-Shoc-Error"))
        {
            var errors = message.Content.ReadFromJsonAsync<AggregateErrorDefinition>().Result;

            throw new ShocException(errors?.Errors ?? new List<ErrorDefinition>());
        }

        // for rest of the errors
        message.EnsureSuccessStatusCode();

        return message;
    }

    /// <summary>
    /// Maps the response on success
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="response">The response to map</param>
    /// <param name="options">The map options</param>
    /// <returns></returns>
    public static Task<T> Map<T>(this HttpResponseMessage response, MapOptions<T> options = null)
    {
        // try get handler
        var handler = options?.GetStatusHandler((int)response.StatusCode);

        // handle if external handler is given
        if(handler != null)
        {
            return handler.Invoke(response);
        }

        // make sure all is fine as we need
        response.EnsureShocSuccess();

        // map from json
        return response.Content.ReadFromJsonAsync<T>();
    }
}
