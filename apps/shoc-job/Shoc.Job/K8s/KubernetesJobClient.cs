using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Shoc.Core.Kubernetes;
using Shoc.Job.K8s.Model;
using Shoc.Job.Model.Job;
using Shoc.Job.Model.JobTask;

namespace Shoc.Job.K8s;

/// <summary>
/// The kubernetes job client
/// </summary>
public class KubernetesJobClient
{
    /// <summary>
    /// The unspecific container registry
    /// </summary>
    private const string UNSPECIFIED_CONTAINER_REGISTRY = "docker.io";
    
    /// <summary>
    /// The underlying kubernetes client
    /// </summary>
    private readonly Kubernetes client;

    /// <summary>
    /// The kubernetes client for job operations
    /// </summary>
    /// <param name="config">The cluster config for authentication</param>
    public KubernetesJobClient(string config)
    {
        this.client = new Kubernetes(new KubeContext { Config = config }.AsClientConfiguration());
    }
    
    /// <summary>
    /// Initializes the namespace object for the job
    /// </summary>
    /// <param name="job">The job object</param>
    /// <returns></returns>
    public async Task<InitNamespaceResult> InitNamespace(JobModel job)
    {
        // create a namespace object
        return new InitNamespaceResult
        {
            Namespace = await this.client.CreateNamespaceAsync(new V1Namespace 
            {
                Metadata = new V1ObjectMeta
                {
                    Name = job.Namespace
                }
            })
        };
    }

    /// <summary>
    /// Initializes the k8s essentials for the given job
    /// </summary>
    /// <param name="job">The job object</param>
    /// <returns></returns>
    public async Task<InitServiceAccountResult> InitServiceAccount(JobModel job)
    {
        // create a ServiceAccount object to work inside the namespace
        var sa = await client.CoreV1.CreateNamespacedServiceAccountAsync(new V1ServiceAccount
        {
            Metadata = new V1ObjectMeta { Name = K8sConstants.SHOC_SERVICE_ACCOUNT_NAME, NamespaceProperty = job.Namespace }
        }, job.Namespace);
        
        // create Role with restrictions (only allows actions within the namespace)
        var role = await client.RbacAuthorizationV1.CreateNamespacedRoleAsync(new V1Role
        {
            Metadata = new V1ObjectMeta { Name = K8sConstants.SHOC_ROLE_NAME, NamespaceProperty = job.Namespace },
            Rules = new List<V1PolicyRule>
            {
                new()
                {
                    ApiGroups = new [] { "" },
                    Resources = new[] { "pods", "services", "configmaps", "secrets", "jobs" },
                    Verbs = new[] { "get", "list", "watch", "create", "update", "delete" }
                }
            }
        }, job.Namespace);
        
        // create a role binding to add service account to the role
        var roleBinding = await client.RbacAuthorizationV1.CreateNamespacedRoleBindingAsync(new V1RoleBinding
        {
            Metadata = new V1ObjectMeta { Name = K8sConstants.SHOC_ROLE_BINDING_NAME, NamespaceProperty = job.Namespace },
            Subjects = new List<Rbacv1Subject>
            {
                new()
                {
                    Kind = "ServiceAccount",
                    Name = sa.Name(),
                    NamespaceProperty = job.Namespace
                }
            },
            RoleRef = new V1RoleRef
            {
                ApiGroup = "rbac.authorization.k8s.io",
                Kind = "Role",
                Name = role.Name()
            }
        }, job.Namespace);

        return new InitServiceAccountResult
        {
            ServiceAccount = sa,
            Role = role,
            RoleBinding = roleBinding
        };
    }
    
    /// <summary>
    /// Initialize the shared environment such as secrets and config maps
    /// </summary>
    /// <param name="job">The job object</param>
    /// <param name="envs">The environment input</param>
    /// <returns></returns>
    public async Task<InitSharedEnvsResult> InitSharedEnvironment(JobModel job, JobTaskEnvModel envs)
    {
        // creates a new secret for encrypted values
        var secret = await this.client.CreateNamespacedSecretAsync(new V1Secret
        {
            Immutable = null,
            Metadata = new V1ObjectMeta
            {
                Name = "shared-secret-all",
                NamespaceProperty = job.Namespace
            },
            StringData = envs.Encrypted ?? new Dictionary<string, string>(),
            Type = "Opaque"
        }, job.Namespace);
        
        // creates a new config map for plain values
        var configMap = await this.client.CreateNamespacedConfigMapAsync(new V1ConfigMap
        {
            Immutable = null,
            Metadata = new V1ObjectMeta
            {
                Name = "shared-configmap-all",
                NamespaceProperty = job.Namespace
            },
            Data = envs.Plain
        }, job.Namespace);

        return new InitSharedEnvsResult
        {
            Secrets = [secret],
            ConfigMaps = [configMap]
        };
    }
    
    /// <summary>
    /// Initialize the pull secret for the target image
    /// </summary>
    /// <param name="job">The job object</param>
    /// <param name="packageReference">The package reference</param>
    /// <returns></returns>
    public async Task<InitPullSecretResult> InitPullSecret(JobModel job, JobTaskPackageReferenceModel packageReference)
    {
        // resolve the registry
        var registry = ResolveRegistry(packageReference.Image);
        
        // build the docker config JSON object
        var dockerConfigJson = JsonSerializer.Serialize(new
        {
            auths = new Dictionary<string, object>
            {
                [registry] = new
                {
                    auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{packageReference.PullUsername}:{packageReference.PullPasswordPlain}"))
                }
            }
        });
        
        // create and return the result
        return new InitPullSecretResult
        {
            PullSecret = await this.client.CreateNamespacedSecretAsync(new V1Secret
            {
                Metadata = new V1ObjectMeta
                {
                    Name = "shared-pull-secret",
                    NamespaceProperty = job.Namespace
                },
                Type = "kubernetes.io/dockerconfigjson",
                Data = new Dictionary<string, byte[]>
                {
                    { ".dockerconfigjson", Encoding.UTF8.GetBytes(dockerConfigJson) }
                }
            }, job.Namespace),
            Image = packageReference.Image
        };
    }

    /// <summary>
    /// Performs a K8s action with namespace cleanup fallback
    /// </summary>
    /// <param name="ns">The namespace</param>
    /// <param name="action">The action to perform</param>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <returns></returns>
    public async Task<TResult> WithCleanup<TResult>(string ns, Func<Task<TResult>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception)
        {
            await this.client.DeleteNamespaceAsync(ns);
            throw;
        }
    }
    
    /// <summary>
    /// Resolves the registry URL by the given image
    /// </summary>
    /// <param name="image">The image</param>
    /// <returns></returns>
    private static string ResolveRegistry(string image)
    {
        // split the image path
        var parts = image.Split('/');

        // consider unspecified
        if (parts.Length == 1)
        {
            return UNSPECIFIED_CONTAINER_REGISTRY;
        }

        // if it contains a domain (e.g., ghcr.io/org/image or registry.example.com/image)
        return parts[0].Contains('.') || parts[0].Contains(':') ? parts[0] : UNSPECIFIED_CONTAINER_REGISTRY;
    }
}