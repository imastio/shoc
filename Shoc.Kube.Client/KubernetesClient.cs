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
                Kind = KubernetesKind.NAMESPACE,
                Metadata = new V1ObjectMeta(name:this.ns, labels:new Dictionary<string, string>{ {"name", this.ns} }),
            });
        }

        /// <summary>
        /// Creates pull secret
        /// </summary>
        /// <param name="config">The config byte array</param>
        /// <returns></returns>
        public async Task CreatePullSecret(byte[] config)
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
        /// Creates job
        /// </summary>
        /// <param name="imageUri"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task CreateJobWithOverridenCommand(string imageUri, IList<string> args)
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
                                    SecurityContext = new V1SecurityContext
                                    {
                                        RunAsUser = 1000
                                    },
                                    Command = args,
                                    VolumeMounts = new List<V1VolumeMount>
                                    {
                                        new()
                                        {
                                            Name = "config-volume",
                                            MountPath = "/home/shoc/hosts"
                                        }
                                    }
                                }
                            },
                            Volumes = new List<V1Volume>
                            {
                                new ()
                                {
                                    Name = "config-volume",
                                    ConfigMap = new V1ConfigMapVolumeSource
                                    {
                                        Name = $"config-{this.jobId}"
                                    }
                                }
                            },
                            ImagePullSecrets = new List<V1LocalObjectReference> { new($"{this.jobId}-secret") },
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
        /// Creates configmap for given key value configs
        /// </summary>
        /// <returns></returns>
        public async Task CreateConfigMap(CreateConfigMapInput input)
        {
            var configMap = new V1ConfigMap()
            {
                ApiVersion = "v1",
                Kind = KubernetesKind.CONFIGMAP,
                Metadata = new V1ObjectMeta
                {
                    Name = input.Name
                },
                Data = input.Configs
            };

            try
            {
                await this.client.CreateNamespacedConfigMapAsync(configMap, this.jobId);
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
            // get logs of the specified pod as stream
            var result = await this.client.CoreV1.ReadNamespacedPodLogWithHttpMessagesAsync(name, this.ns, null, true);

            return result.Body;
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
        /// Creates headless service with given port
        /// </summary>
        /// <param name="input">The create headless service model</param>
        /// <returns></returns>
        public async Task CreateHeadlessService(CreateHeadlessServiceInput input)
        {
            var service = new V1Service
            {
                ApiVersion = "v1",
                Kind = KubernetesKind.SERVICE,
                Metadata = new V1ObjectMeta
                {
                    Name = input.Name
                },
                Spec = new V1ServiceSpec
                {
                    ClusterIP = "None",
                    Selector = input.Selector,
                    Ports = new List<V1ServicePort>
                    {
                        new()
                        {
                            Name = input.Name,
                            Port = input.Port,
                            TargetPort = input.TargetPort
                        }
                    }
                }
            };

            try
            {
                await this.client.CreateNamespacedServiceAsync(service, this.jobId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
            }
        }

        /// <summary>
        /// Creates statefulset
        /// </summary>
        /// <returns></returns>
        public async Task CreateStatefulSet(CreateStatefulSetInput input)
        {
            var statefulSet = new V1StatefulSet
            {
                ApiVersion = "apps/v1",
                Kind = KubernetesKind.STATEFULSET,
                Metadata = new V1ObjectMeta
                {
                    Name = input.Name,
                },
                Spec = new V1StatefulSetSpec
                {
                    ServiceName = input.ServiceName,
                    Selector = new V1LabelSelector
                    {
                        MatchLabels = input.ServiceSelector
                    },
                    Replicas = input.Replicas,
                    Template = new V1PodTemplateSpec
                    {
                        Metadata = new V1ObjectMeta
                        {
                            Labels = input.PodLabels
                        },
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>
                            {
                                new()
                                {
                                    Name = input.ContainerName,
                                    Image = input.ContainerImageUri,
                                    ImagePullPolicy = "Always",
                                    Ports = new List<V1ContainerPort>
                                    {
                                        new()
                                        {
                                            ContainerPort = input.ContainerPort
                                        }
                                    }
                                }
                            }
                        }
                    },
                }
            };

            try
            {
                await this.client.CreateNamespacedStatefulSetAsync(statefulSet, this.jobId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
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
