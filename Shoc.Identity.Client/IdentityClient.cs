using System.Collections.Generic;
using Imast.Ext.DiscoveryCore;
using Shoc.ClientCore;
using Shoc.Core;
using System.Net.Http;
using System.Threading.Tasks;
using Shoc.Identity.Model;

namespace Shoc.Identity.Client
{
    /// <summary>
    /// The identity api client
    /// </summary>
    public class IdentityClient : ShocApiClient
    {
        /// <summary>
        /// The service name by default
        /// </summary>
        private const string DEFAULT_SERVICE = "shoc-identity";

        /// <summary>
        /// Creates new instance of the client
        /// </summary>
        /// <param name="client">The client name</param>
        /// <param name="service">The service</param>
        /// <param name="discovery">The discovery</param>
        public IdentityClient(string client, string service, IDiscoveryClient discovery) : base(client, service ?? DEFAULT_SERVICE, discovery)
        {
        }

        /// <summary>
        /// Creates new instance of the client
        /// </summary>
        /// <param name="client">The client name</param>
        /// <param name="discovery">The discovery</param>
        public IdentityClient(string client, IDiscoveryClient discovery) : base(client, DEFAULT_SERVICE, discovery)
        {
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="token">The access token</param>
        /// <returns></returns>
        public async Task<IEnumerable<UserModel>> GetUsers(string token)
        {
            // the url of api
            var url = await this.GetApiUrl("api/users");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<IEnumerable<UserModel>>();
        }

        /// <summary>
        /// Gets user by email
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="email">The user email</param>
        /// <returns></returns>
        public async Task<UserModel> GetUserByEmail(string token, string email)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/users/by-email/{email}");

            // build the message
            var message = BuildMessage(HttpMethod.Get, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<UserModel>();
        }

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="userModel">The create user model input</param>
        /// <returns></returns>
        public async Task<UserModel> CreateUser(string token, CreateUserModel userModel)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/users");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, userModel, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<UserModel>();
        }

        /// <summary>
        /// Deletes user by identifier
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="id">The user identifier</param>
        /// <returns></returns>
        public async Task<UserModel> DeleteUser(string token, string id)
        {
            // the url of api
            var url = await this.GetApiUrl($"api/users/{id}");

            // build the message
            var message = BuildMessage(HttpMethod.Delete, url, null, Auth(token));

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<UserModel>();
        }

        /// <summary>
        /// Creates a new root user entity
        /// </summary>
        /// <param name="input">The input to create</param>
        /// <returns></returns>
        public async Task<UserModel> CreateRoot(CreateRootModel input)
        {
            // the url of api
            var url = await this.GetApiUrl("api/setup/root");

            // build the message
            var message = BuildMessage(HttpMethod.Post, url, input);

            // execute safely and get response
            var response = await Guard.DoAsync(() => this.webClient.SendAsync(message));

            // get the result
            return await response.Map<UserModel>();
        }
    }
}