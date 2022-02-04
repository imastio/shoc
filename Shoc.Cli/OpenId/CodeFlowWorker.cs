using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shoc.Cli.OpenId
{
    /// <summary>
    /// The code flow worker to get response
    /// </summary>
    public class CodeFlowWorker
    {
        /// <summary>
        /// The code flow options
        /// </summary>
        private readonly CodeFlowOptions options;

        /// <summary>
        /// The listener of http requests
        /// </summary>
        private readonly HttpListener listener;

        /// <summary>
        /// Creates new instance of code flow worker 
        /// </summary>
        /// <param name="options">The options of worker</param>
        public CodeFlowWorker(CodeFlowOptions options)
        {
            this.options = options;
            this.listener = new HttpListener
            {
                Prefixes = { $"http://127.0.0.1:{this.options.Port}/" }
            };
        }

        /// <summary>
        /// The method to wait for the code flow response
        /// </summary>
        /// <returns></returns>
        public async Task<CodeFlowResponse> Get()
        {
            // start listening
            this.listener.Start();

            // wait for one of tasks to complete
            var completed = await Task.WhenAny(this.RequestWaiter(), this.TimeoutWaiter());

            // return result of completed one
            return await completed;
        }

        /// <summary>
        /// A method to wait for an incoming http request to handle
        /// </summary>
        /// <returns></returns>
        private async Task<CodeFlowResponse> RequestWaiter()
        {
            try
            {
                // wait for context to come (incoming request)
                var context = await this.listener.GetContextAsync();

                // the response string
                var responseString = string.Empty;

                // the content type
                var contentType = context.Request.ContentType ?? string.Empty;

                // if content type is url encoded
                var isUrlEncoded = "application/x-www-form-urlencoded".Equals(contentType, StringComparison.OrdinalIgnoreCase);

                // success by default
                var statusCode = 200;

                // process based on method 
                switch (context.Request.HttpMethod)
                {
                    // in case of get request try get from query string
                    case "GET":
                    {
                        // get from query string
                        responseString = context.Request.Url?.Query ?? string.Empty;
                        break;
                    }

                    // in case of post request get from 
                    case "POST" when isUrlEncoded:
                    {
                        // create a reader to read the body
                        using var reader = new StreamReader(context.Request.InputStream, Encoding.UTF8);

                        // read the content
                        responseString = await reader.ReadToEndAsync();
                        break;
                    }

                    // in case if post but not url encoded
                    case "POST":
                    {
                        // if not url encoded type report the error
                        statusCode = 415;
                        break;
                    }

                    // not supported by default
                    default:
                    {
                        statusCode = 405;
                        break;
                    }
                }
                
                // return response 
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "text/html";

                // the output writer
                await using var writer = new StreamWriter(context.Response.OutputStream);

                // message on success
                if (statusCode == 200)
                {
                    await writer.WriteLineAsync("Success! You can close the browser now.");
                }
                else
                {
                    await writer.WriteLineAsync("Something went wrong!");
                }

                // return the result
                return new CodeFlowResponse
                {
                    Error = string.Empty,
                    Success = statusCode == 200,
                    Timeout = false,
                    ResponseString = responseString
                };
            }
            catch (Exception e)
            {
                // in case of any error 
                return new CodeFlowResponse
                {
                    Success = false,
                    Timeout = false,
                    Error = e.Message,
                    ResponseString = string.Empty
                };
            }
        }

        /// <summary>
        /// The method to wait for the timeout and break from the worker
        /// </summary>
        /// <returns></returns>
        private async Task<CodeFlowResponse> TimeoutWaiter()
        {
            // wait for the timeout to happen
            await Task.Delay(this.options.Timeout);

            // return timeout response
            return new CodeFlowResponse
            {
                Timeout = true,
                Success = false,
                Error = "Timeout",
                ResponseString = string.Empty
            };
        }
    }
}