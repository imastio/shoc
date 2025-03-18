using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shoc.Job.Services;

/// <summary>
/// The log streaming service
/// </summary>
public class LogStreamingService
{
    /// <summary>
    /// Redirect given source stream into the target response stream as SSE
    /// </summary>
    /// <param name="source">The source stream</param>
    /// <param name="target">The target response</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public async Task StreamLogs(Stream source, HttpResponse target, CancellationToken cancellationToken)
    {
        // set the response content type to text/event-stream
        target.ContentType = "text/event-stream";
        target.Headers.CacheControl = "no-cache";
        target.Headers.KeepAlive = "keep-alive";
        
        // open a reader
        using var reader = new StreamReader(source);

        try
        {
            // process line by line
            await foreach (var line in ReadLinesAsync(reader, cancellationToken))
            {
                await target.WriteAsync($"data: {line}\n\n", cancellationToken);
                await target.Body.FlushAsync(cancellationToken);
            }
        }
        catch (Exception)
        {
            // ignored
        }
        finally
        {
            await source.DisposeAsync();
        }
    }
    
    /// <summary>
    /// Yield lines from the reader until the end
    /// </summary>
    /// <param name="reader">The reader to process</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    private static async IAsyncEnumerable<string> ReadLinesAsync(TextReader reader, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            yield return line;
        }
    }
}