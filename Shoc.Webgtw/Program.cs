using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Shoc.Webgtw
{
    /// <summary>
    /// The program default implementation
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point 
        /// </summary>
        /// <param name="args">The arguments</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        /// <summary>
        /// Creates a host builder
        /// </summary>
        /// <param name="args">The arguments</param>
        /// <returns></returns>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureAppConfiguration((_, config) => config.AddEnvironmentVariables());
    }
}