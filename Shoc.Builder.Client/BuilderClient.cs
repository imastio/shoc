using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Imast.Ext.DiscoveryCore;
using Shoc.Builder.Model.Kubernetes;
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
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

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
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

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

        /// <summary>
        /// Gets all kubernetes cluster
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="name">The name of the cluster</param>
        /// <returns></returns>
        public async Task<IEnumerable<KubernetesCluster>> GetClusters(string token, string name = null)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/kubernetes-clusters/?name={name}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<IEnumerable<KubernetesCluster>>();
        }

        /// <summary>
        /// Creates a new cluster entity
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="input">The input to create</param>
        /// <returns></returns>
        public async Task<KubernetesCluster> CreateCluster(string token, CreateKubernetesCluster input)
        {
            // the url of api
            var url = await this.GetApiUrl("api/kubernetes-clusters");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, input, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<KubernetesCluster>();
        }

        /// <summary>
        /// Deletes existing cluster entity
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The id of the cluster</param>
        /// <returns></returns>
        public async Task<KubernetesCluster> DeleteCluster(string token, string id)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/kubernetes-clusters/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Delete, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<KubernetesCluster>();
        }
    }
}
