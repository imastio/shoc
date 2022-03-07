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
        public async Task<IEnumerable<ProjectModel>> GetAllProjects(string token)
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
        public async Task<ProjectModel> CreateProject(string token, CreateUpdateProjectModel input)
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
        /// Updates a new project entity
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The id of object to update</param>
        /// <param name="input">The input to create</param>
        /// <returns></returns>
        public async Task<ProjectModel> UpdateProject(string token, string id, CreateUpdateProjectModel input)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Put, url, input, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<ProjectModel>();
        }

        /// <summary>
        /// Gets all the projects owned by me
        /// </summary>
        /// <param name="token">The access token</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectModel>> GetMyAllProjects(string token)
        {
            // the url of api
            var url = await this.GetApiUrl("api/my-projects");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<IEnumerable<ProjectModel>>();
        }

        /// <summary>
        /// Gets the owned project by given id
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The id of project</param>
        /// <returns></returns>
        public async Task<ProjectModel> GetMyProjectById(string token, string id)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/my-projects/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map(new MapOptions<ProjectModel>().OnNotFoundDefault());
        }

        /// <summary>
        /// Creates a new project entity owned by me
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="input">The input to create</param>
        /// <returns></returns>
        public async Task<ProjectModel> CreateMyProject(string token, CreateUpdateProjectModel input)
        {
            // the url of api
            var url = await this.GetApiUrl("api/my-projects");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, input, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<ProjectModel>();
        }

        /// <summary>
        /// Updates a new project entity owned by me
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The id of object to update</param>
        /// <param name="input">The input to create</param>
        /// <returns></returns>
        public async Task<ProjectModel> UpdateMyProject(string token, string id, CreateUpdateProjectModel input)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/my-projects/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Put, url, input, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<ProjectModel>();
        }
    }
}
