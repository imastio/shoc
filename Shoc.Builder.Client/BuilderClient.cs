using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Imast.Ext.DiscoveryCore;
using Shoc.Builder.Model.Project;
using Shoc.ClientCore;
using Shoc.Core;

namespace Shoc.Builder.Client
{
    /// <summary>
    /// The builder api client
    /// </summary>
    public class BuilderClient : ShocApiClient
    {
        /// <summary>
        /// The service name by default
        /// </summary>
        private static readonly string DEFAULT_SERVICE = "shoc-builder";

        /// <summary>
        /// Creates new instance of the client
        /// </summary>
        /// <param name="client">The client name</param>
        /// <param name="discovery">The discovery</param>
        public BuilderClient(string client, IDiscoveryClient discovery) : base(client, DEFAULT_SERVICE, discovery)
        {
        }

        /// <summary>
        /// Gets all the projects
        /// </summary>
        /// <param name="token">The access token</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectModel>> GetProjects(string token)
        {
            // the url of api
            var url = await this.GetApiUrl("api/projects");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<IEnumerable<ProjectModel>>();
        }

        /// <summary>
        /// Gets all the projects by name and directory
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="directory">The directory</param>
        /// <param name="name">The name</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectModel>> GetProjectsByPath(string token, string directory, string name)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects?directory={directory}&name={name}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<IEnumerable<ProjectModel>>();
        }

        /// <summary>
        /// Gets the project by given id
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectById(string token, string id)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map(new MapOptions<ProjectModel>().OnNotFoundDefault());
        }

        /// <summary>
        /// Creates a new project entity
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="input">The input to create</param>
        /// <returns></returns>
        public async Task<ProjectModel> CreateProject(string token, CreateProjectModel input)
        {
            // the url of api
            var url = await this.GetApiUrl("api/projects");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, input, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<ProjectModel>();
        }
        
        /// <summary>
        /// Deletes the project by id
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        public async Task<ProjectModel> DeleteProjectById(string token, string id)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Delete, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<ProjectModel>();
        }
    }
}
