using Imast.Ext.DiscoveryCore;
using Shoc.ClientCore;
using Shoc.Core;
using System.Net.Http;
using System.Threading.Tasks;
using Shoc.Executor.Model.Job;
using Shoc.Executor.Model.Kubernetes;
using System.Collections.Generic;

namespace Shoc.Executor.Client
{
    /// <summary>
    /// The executor api client
    /// </summary>
    public class ExecutorClient : ShocApiClient
    {
        /// <summary>
        /// The service name by default
        /// </summary>
        private static readonly string DEFAULT_SERVICE = "shoc-executor";

        /// <summary>
        /// Creates new instance of the client
        /// </summary>
        /// <param name="client">The client name</param>
        /// <param name="discovery">The discovery</param>
        public ExecutorClient(string client, IDiscoveryClient discovery) : base(client, DEFAULT_SERVICE, discovery)
        {
        }

        /// <summary>
        /// Run the project
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="input">The job creation input</param>
        /// <returns></returns>
        public async Task<JobModel> CreateProjectJob(string token, CreateJobInput input)
        {
            // the url of api
            var url = await this.GetApiUrl("api/jobs");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, input, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<JobModel>();
        }

        /// <summary>
        /// Deploy the project
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The job id</param>
        /// <returns></returns>
        public async Task<JobModel> DeployProject(string token, string id)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/jobs/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<JobModel>();
        }

        /// <summary>
        /// Deploy the project
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The job id</param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> WatchJob(string token, string id)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/jobs/{id}/watch");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<Dictionary<string, object>>();
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
