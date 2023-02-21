using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Shoc.Kube.Model;

namespace Shoc.Kube.Client
{
    /// /// <summary>
    /// The kubernetes client
    /// </summary>
    public class KubernetesClient
    {
        /// <summary>
        /// The kubernetes client
        /// </summary>
        private readonly Kubernetes client;

        /// <summary>
        /// The default namespace for kubernetes
        /// </summary>
        private readonly string ns;

        /// <summary>
        /// The default namespace for kubernetes
        /// </summary>
        private readonly string jobId;

        /// <summary>
        /// Creates new instance of kubernetes client
        /// </summary>
        public KubernetesClient(string kubeConfig, string jobId)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(kubeConfig));

            this.client = new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile(stream));
            this.jobId = jobId;
            this.ns = jobId;
        }

        /// <summary>
        /// Assures namespace exists
        /// </summary>
        /// <returns></returns>
        public async Task AssureNamespace()
        {
            // get all namespaces
            var allNamespaces = await this.client.CoreV1.ListNamespaceAsync();

            // try get out namespace 
            var existingNs = allNamespaces.Items.FirstOrDefault(n => n.Metadata.Name == this.ns);

            // if exists
            if (existingNs != null)
            {
                return;
            }

            // create namespace
            await this.client.CreateNamespaceAsync(new V1Namespace
            {
                //ApiVersion = "v1",
                Kind = KubernetesKind.NAMESPACE,
                Metadata = new V1ObjectMeta(name:this.ns, labels:new Dictionary<string, string>{ {"name", this.ns} }),
            });
        }

        /// <summary>
        /// Creates secret kind
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task CreateSecret(byte[] config)
        {
            var secret = new V1Secret
            {
                ApiVersion = "v1",
                Kind = KubernetesKind.SECRET,
                Metadata = this.GetMetadata(),
                Data = new Dictionary<string, byte[]>
                {
                    { ".dockerconfigjson", config }
                },
                Type = "kubernetes.io/dockerconfigjson"
            };

            try
            {
                await this.client.CreateNamespacedSecretAsync(secret, this.ns);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
            }
        }

        /// <summary>
        /// Creates job
        /// </summary>
        /// <param name="imageUri"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task CreateJob(string imageUri, IEnumerable<string> args)
        {
            var job = new V1Job
            {
                ApiVersion = "batch/v1",
                Kind = KubernetesKind.JOB,
                Metadata = new V1ObjectMeta
                {
                    Labels = this.GetLabels(),
                    Name = $"{this.jobId}",
                },
                Spec = new V1JobSpec
                {
                    Template = new V1PodTemplateSpec
                    {
                        Metadata = new V1ObjectMeta
                        {
                            Labels = this.GetLabels()
                        },
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>
                            {
                                new()
                                {
                                    Image = imageUri,
                                    Name = this.jobId,
                                    Args = args?.ToList()
                                }
                            },
                            ImagePullSecrets = new List<V1LocalObjectReference> { new($"{this.jobId}-secret")},
                            RestartPolicy = "Never"
                        }
                    },
                }
            };

            try
            {
                await this.client.CreateNamespacedJobAsync(job, this.jobId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
            }
        }

        /// <summary>
        /// Watches job
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> GetPodLog(string name)
        {
            try
            {
                var result =  await this.client.CoreV1.ReadNamespacedPodLogWithHttpMessagesAsync(name, this.ns, null, true);

                return result.Body;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);

                return Stream.Null;
            }
        }

        /// <summary>
        /// Watches job
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetPodsByLabel(string key, string value)
        {
            try
            {
                var pods = await this.client.CoreV1.ListNamespacedPodAsync(this.ns, labelSelector: $"{key}={value}");

                return pods.Items.Select(p => p.Name());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
                
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Get metadata object
        /// </summary>
        /// <returns></returns>
        private V1ObjectMeta GetMetadata()
        {
            return new V1ObjectMeta
            {
                Labels = this.GetLabels(),
                Name = $"{this.jobId}-secret"
            };
        }

        /// <summary>
        /// Get labels for metadata
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetLabels()
        {
            return new Dictionary<string, string>()
            {
                { "app" , this.jobId }
            };
        }
    }
}
