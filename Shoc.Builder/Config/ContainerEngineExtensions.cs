using System;
using Docker.DotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shoc.ApiCore;
using Shoc.Engine.Client;

namespace Shoc.Builder.Config
{
    /// <summary>
    /// The extension methods for container engines
    /// </summary>
    public static class ContainerEngineExtensions
    {
        /// <summary>
        /// Adds the container engine for future use
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddEngineClient(this IServiceCollection services, IConfiguration configuration)
        {
            // get engine client settings
            var engineSettings = configuration.BindAs<EngineClientSettings>("EngineClient");

            // keep settings for future use
            services.AddSingleton(engineSettings);

            // create an instance of docker client
            var dockerClient = new DockerClientConfiguration(new Uri(engineSettings.Address))
                .CreateClient(new Version(engineSettings.Version));

            // add docker client implementation
            services.AddSingleton<IDockerClient>(dockerClient);

            // add engine client 
            services.AddSingleton<EngineClient>();

            // continue chaining
            return services;
        }
    }
}