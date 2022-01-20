using System.Threading.Tasks;
using Docker.DotNet;
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
    }
}
