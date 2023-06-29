using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Imast.Ext.DiscoveryCore;
using Shoc.Builder.Model.Package;
using Shoc.Builder.Model.Project;
using Shoc.Builder.Model.Registry;
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
        private const string DEFAULT_SERVICE = "shoc-builder";

        /// <summary>
        /// Creates new instance of the client
        /// </summary>
        /// <param name="client">The client name</param>
        /// <param name="discovery">The discovery</param>
        public BuilderClient(string client, IDiscoveryClient discovery) : base(client, DEFAULT_SERVICE, discovery)
        {
            this.webClient.Timeout = Timeout.InfiniteTimeSpan;
        }

        /// <summary>
        /// Gets all the projects
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="name">The project name filter</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectModel>> GetProjects(string token, string name = null)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects?name={name}");

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
        /// <param name="ownerId">The owner id</param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectById(string token, string id, string ownerId)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{id}/by-owner/{ownerId}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map(new MapOptions<ProjectModel>().OnNotFoundDefault());
        }

        /// <summary>
        /// Gets the package by given id
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="projectId">The project id</param>
        /// <param name="id">The id of project</param>
        /// <param name="ownerId">The owner id</param>
        /// <returns></returns>
        public async Task<ShocPackage> GetPackageById(string token, string projectId, string id, string ownerId)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{projectId}/packages/{id}/by-owner/{ownerId}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map(new MapOptions<ShocPackage>().OnNotFoundDefault());
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

        /// <summary>
        /// Gets all the projects
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="projectId">The project id</param>
        /// <returns></returns>
        public async Task<IEnumerable<ShocPackage>> GetPackages(string token, string projectId)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{projectId}/packages");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<IEnumerable<ShocPackage>>();
        }

        /// <summary>
        /// Creates a new package entity
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="projectId">The id of project</param>
        /// <param name="input">The input to create</param>
        /// <returns></returns>
        public async Task<ShocPackage> CreatePackage(string token, string projectId, CreatePackageInput input)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{projectId}/packages");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, input, Auth(token));

            // execute safely and get response
            var response = await WithTimeout(TimeSpan.FromMinutes(10), source => Guard.DoAsync(() => this.webClient.SendAsync(message, source.Token)));
            
            // get the result
            return await response.Map<ShocPackage>();
        }

        /// <summary>
        /// Upload the bundle
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="projectId">The id of project</param>
        /// <param name="id">The id of the package</param>
        /// <param name="file">The file to upload</param>
        /// <returns></returns>
        public async Task<PackageBundleReference> UploadBundle(string token, string projectId, string id, string file)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{projectId}/packages/{id}/bundle");

            // build the message
            var message = BuildMessage(HttpMethod.Put, url, null, Auth(token));

            // open the file stream for reading
            await using var stream = new FileStream(file, FileMode.Open, FileAccess.Read);

            // build the content for sending the bundle
            using var content = new MultipartFormDataContent
            {
                {new StreamContent(stream), "file", "bundle.zip"}
            };

            // build message content
            message.Content = content;
            
            // execute safely and get response
            var response = await WithTimeout(TimeSpan.FromMinutes(10), source => Guard.DoAsync(() => this.webClient.SendAsync(message, source.Token)));

            // get the result
            return await response.Map<PackageBundleReference>();
        }

        /// <summary>
        /// Build the package
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="projectId">The id of project</param>
        /// <param name="id">The id of the package</param>
        /// <param name="version">The version of the package</param>
        /// <returns></returns>
        public async Task<PackageBundleReference> BuildPackage(string token, string projectId, string id, string version)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{projectId}/packages/{id}/build/{version}");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<PackageBundleReference>();
        }

        /// <summary>
        /// Get project version by name
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="projectId">The id of project</param>
        /// <param name="version">The version of the package</param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectVersion>> GetProjectVersionByName(string token, string projectId, string version)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/projects/{projectId}/versions?version={version}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<IEnumerable<ProjectVersion>>();
        }

        /// <summary>
        /// Gets all docker registries
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="name">The name of the registry</param>
        /// <returns></returns>
        public async Task<IEnumerable<DockerRegistry>> GetRegistries(string token, string name = null)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/docker-registries/?name={name}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<IEnumerable<DockerRegistry>>();
        }

        /// <summary>
        /// Gets docker registry by id
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The id of the registry</param>
        /// <returns></returns>
        public async Task<DockerRegistry> GetRegistryById(string token, string id)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/docker-registries/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<DockerRegistry>();
        }

        /// <summary>
        /// Creates a new registry entity
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="input">The input to create</param>
        /// <returns></returns>
        public async Task<DockerRegistry> CreateRegistry(string token, CreateDockerRegistry input)
        {
            // the url of api
            var url = await this.GetApiUrl("api/docker-registries");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, input, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<DockerRegistry>();
        }

        /// <summary>
        /// Deletes existing registry entity
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The id of the registry</param>
        /// <returns></returns>
        public async Task<DockerRegistry> DeleteRegistry(string token, string id)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/docker-registries/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Delete, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<DockerRegistry>();
        }
    }
}
