using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;

namespace Shoc.Cli.OpenId
{
    /// <summary>
    /// The system browser implementation
    /// </summary>
    public class SystemBrowser : IBrowser
    {
        /// <summary>
        /// The given port to use
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// The port for browser to use
        /// </summary>
        /// <param name="port">The port to use</param>
        public SystemBrowser(int port)
        {
            this.Port = port;
        }

        /// <summary>
        /// Invokes the browser and gets the result
        /// </summary>
        /// <param name="options">The browser options</param>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns></returns>
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = new())
        {
            // the create a worker
            var worker = new CodeFlowWorker(new CodeFlowOptions
            {
                Port = this.Port,
                Timeout = options.Timeout,
            });

            // wait a sec
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

            // start the browser
            this.StartBrowser(options.StartUrl);
            
            // wait for the result of flow
            var result = await worker.Get();

            // the type of result
            var resultType = BrowserResultType.Success;
            
            // not a success
            if (!result.Success)
            {
                resultType = BrowserResultType.UnknownError;
            }

            // timed out
            if (result.Timeout)
            {
                resultType = BrowserResultType.Timeout;
            }

            // return the result
            return new BrowserResult {Response = result.ResponseString, ResultType = resultType, Error = result.Error };
        }


        /// <summary>
        /// Start the system browser with given URL
        /// </summary>
        /// <param name="url">The URL to open</param>
        public void StartBrowser(string url)
        {
            try
            {
                // start as a separate process
                Process.Start(url);
            }
            catch
            {
                // if anything went wrong try rely on platform specific command line
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}