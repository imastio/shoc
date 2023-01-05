using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Shoc.Engine.Model;

namespace Shoc.Engine.Client
{
    /// <summary>
    /// The container engine client
    /// </summary>
    public class EngineClient
    {
        /// <summary>
        /// The engine client settings
        /// </summary>
        private readonly EngineClientSettings settings;

        /// <summary>
        /// The docker client
        /// </summary>
        private readonly IDockerClient dockerClient;

        /// <summary>
        /// Creates new instance of registry client
        /// </summary>
        /// <param name="settings">The registry client settings</param>
        /// <param name="dockerClient">The docker client</param>
        public EngineClient(EngineClientSettings settings, IDockerClient dockerClient)
        {
            this.settings = settings;
            this.dockerClient = dockerClient;
        }

        /// <summary>
        /// Gets the info about reference engine
        /// </summary>
        /// <returns></returns>
        public async Task<EngineInstanceInfo> GetInfo()
        {
            // try get the system info
            var info = await this.dockerClient.System.GetSystemInfoAsync();

            // build and return instance info
            return new EngineInstanceInfo
            {
                Id = info.ID,
                Name = info.Name,
                Driver = info.Driver,
                Running = true,
                Images = info.Images,
                Containers = info.Containers
            };
        }

        /// <summary>
        /// Creates the image
        /// </summary>
        /// <param name="input">The creation input</param>
        /// <returns></returns>
        public async Task CreateImage(ImageBuildInput input)
        {
            // try get the system info
            await this.dockerClient.Images.BuildImageFromDockerfileAsync(
                new ImageBuildParameters
                {
                    Tags = new List<string> { $"{input.ImageUri}:{input.Version}" }
                }, 
                input.Payload, new List<AuthConfig>(), 
                new Dictionary<string, string>(), 
                new Progress<JSONMessage>(message =>
                {
                    Console.WriteLine($"Message of creation: {JsonSerializer.Serialize(message)}");
                })
            );
        }

        /// <summary>
        /// Pushes the image to registry
        /// </summary>
        /// <param name="input">The creation input</param>
        /// <returns></returns>
        public async Task PushImage(ImagePushInput input)
        {
            // try get the system info
            await this.dockerClient.Images.PushImageAsync(input.ImageUri, new ImagePushParameters
                {
                    Tag = input.Version
                },
                new AuthConfig
                {
                    Username = input.Username,
                    Password = input.Password
                },
                new Progress<JSONMessage>((message) =>
                {
                    Console.WriteLine($"Message of pushing: {JsonSerializer.Serialize(message)}");
                }));
        }
    }
}
