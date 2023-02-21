using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Shoc.Executor.Model.Deployment;
using Shoc.Executor.Model.Job;
using Shoc.Kube.Client;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Shoc.Executor.Services
{
    /// <summary>
    /// Base class for deployments
    /// </summary>
    public class DeploymentBase
    {
        /// <summary>
        /// The kubernetes client
        /// </summary>
        protected readonly KubernetesClient kubeClient;

        /// <summary>
        /// The kubernetes client
        /// </summary>
        protected readonly CreateDeploymentInput deploymentInput;

        /// <summary>
        /// Creates new instance of class
        /// </summary>
        /// <param name="input">The input of deployment</param>
        public DeploymentBase(CreateDeploymentInput input)
        {
            this.deploymentInput = input;
            this.kubeClient = new KubernetesClient(input.Kubeconfig, input.Job.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual async Task AssureNamespace()
        {
            // make sure required namespace exists
            await kubeClient.AssureNamespace();
        }

        /// <summary>
        /// Creates secret in kubernetes
        /// </summary>
        /// <returns></returns>
        protected virtual async Task CreateSecret()
        {
            // if either username or password not set then consider secret not needed
            if (string.IsNullOrEmpty(deploymentInput.RegistryUsername) || string.IsNullOrEmpty(deploymentInput.RegistryPassword))
            {
                return;
            }

            // get the config as byte array
            var config = CreateSecretConfig(deploymentInput.RegistryUsername, deploymentInput.RegistryPassword, 
                deploymentInput.RegistryEmail, deploymentInput.RegistryUrl);

            // create the secret in kubernetes
            await kubeClient.CreateSecret(config);
        }

        /// <summary>
        /// Creates secret in kubernetes
        /// </summary>
        /// <returns></returns>
        protected virtual async Task CreateJob()
        {
            // create serializer
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            // get run info
            var runInfo = deserializer.Deserialize<JobRunInfo>(deploymentInput.Job.RunInfo);

            await this.kubeClient.CreateJob(deploymentInput.Image, runInfo.Args);
        }

        /// <summary>
        /// Creates config for secret kind
        /// </summary>
        /// <param></param>
        /// <param name="username">The username</param>
        /// <param name="password">The plaintext password</param>
        /// <param name="email">The email address</param>
        /// <param name="uri">The uri</param>
        /// <returns>The serialized config</returns>
        private static byte[] CreateSecretConfig(string username, string password, string email, string uri)
        {
            // encode username and password
            var encodedAuth = Base64Encode($"{username}:{password}");

            // create config object
            var createSecretInput = new Dictionary<string, Dictionary<string, object>>()
            {
                {"auths", new Dictionary<string, object>()
                    {
                        {$"{uri}", new {
                            Username = username,
                            Password = password,
                            Email = email,
                            Auth = encodedAuth
                        }}
                    }
                }
            };

            // return serialized object
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(createSecretInput));
        }

        /// <summary>
        /// Base64 encode given text
        /// </summary>
        /// <param name="text">The text to encode</param>
        /// <returns></returns>
        public static string Base64Encode(string text)
        {
            // return encoded
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }
    }
}
