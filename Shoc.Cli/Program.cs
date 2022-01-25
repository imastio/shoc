using System;
using System.Threading.Tasks;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Shoc.Cli.OpenId;
using Shoc.Cli.System;

namespace Shoc.Cli
{
    /// <summary>
    /// The console application for Shoc interaction
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point of the application
        /// </summary>
        /// <param name="args">The CLI arguments</param>
        public static async Task Main(string[] args)
        {
            var network = new NetworkService();

            var browser = new SystemBrowser(network.GetNextAvailablePort());
            var redirectUri = $"http://127.0.0.1:{browser.Port}";

            var options = new OidcClientOptions
            {
                Authority = "https://localhost:11009",
                ClientId = "native",
                RedirectUri = redirectUri,
                Browser = browser,
                Scope = "openid profile email",
                FilterClaims = false
            };

            var client = new OidcClient(options);
            
            var result = await client.LoginAsync(new LoginRequest
            {
                BrowserTimeout = (int)TimeSpan.FromMinutes(5).TotalSeconds,
                BrowserDisplayMode = DisplayMode.Visible
            });

            if (result.IsError)
            {
                Console.WriteLine($"Error while logging in {result.Error}");
                return;
            }

            Console.WriteLine($"Got token {result.AccessToken}");

            Console.WriteLine("Hello World!");
        }
    }
}
