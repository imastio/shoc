using System.Collections.Generic;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Shoc.Core.Kubernetes;
using Shoc.Job.K8s.Model;
using Shoc.Job.Model.Job;

namespace Shoc.Job.K8s;

/// <summary>
/// The kubernetes job client
/// </summary>
public class KubernetesJobClient
{
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
    /// Initializes the k8s essentials for the given job
    /// </summary>
    /// <param name="job">The job instance</param>
    /// <returns></returns>
    public async Task<K8sJobInitResult> InitJob(JobModel job)
    {
        // the target namespace name
        var ns = job.Id;
        
        // create a namespace object
        var nsObject = await this.client.CreateNamespaceAsync(new V1Namespace 
        {
            Metadata = new V1ObjectMeta { Name = ns }
        });
        
        // create a ServiceAccount object to work inside the namespace
        var sa = await client.CoreV1.CreateNamespacedServiceAccountAsync(new V1ServiceAccount
        {
            Metadata = new V1ObjectMeta { Name = K8sConstants.SHOC_SERVICE_ACCOUNT_NAME, NamespaceProperty = ns }
        }, ns);
        
        // create Role with restrictions (only allows actions within the namespace)
        var role = await client.RbacAuthorizationV1.CreateNamespacedRoleAsync(new V1Role
        {
            Metadata = new V1ObjectMeta { Name = K8sConstants.SHOC_ROLE_NAME, NamespaceProperty = ns },
            Rules = new List<V1PolicyRule>
            {
                new()
                {
                    ApiGroups = new [] { "" },
                    Resources = new[] { "pods", "services", "configmaps", "secrets", "jobs" },
                    Verbs = new[] { "get", "list", "watch", "create", "update", "delete" }
                }
            }
        }, ns);
        
        // create a role binding to add service account to the role
        var roleBinding = await client.RbacAuthorizationV1.CreateNamespacedRoleBindingAsync(new V1RoleBinding
        {
            Metadata = new V1ObjectMeta { Name = K8sConstants.SHOC_ROLE_BINDING_NAME, NamespaceProperty = ns },
            Subjects = new List<Rbacv1Subject>
            {
                new()
                {
                    Kind = "ServiceAccount",
                    Name = sa.Name(),
                    NamespaceProperty = ns
                }
            },
            RoleRef = new V1RoleRef
            {
                ApiGroup = "rbac.authorization.k8s.io",
                Kind = "Role",
                Name = role.Name()
            }
        }, ns);

        return new K8sJobInitResult
        {
            Namespace = nsObject,
            ServiceAccount = sa,
            Role = role,
            RoleBinding = roleBinding
        };
    }
}