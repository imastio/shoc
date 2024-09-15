using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Shoc.Core.Kubernetes;

namespace Shoc.Cluster.Services;

/// <summary>
/// The K8s Service
/// </summary>
public class K8sService
{
    /// <summary>
    /// Test the connection to the Kubernetes cluster
    /// </summary>
    /// <param name="context">The kube context</param>
    /// <returns></returns>
    public async Task<long> GetNodesCount(KubeContext context)
    {
        // Load from the default kubeconfig on the machine.
        var config = context.AsClientConfiguration();

        // creates a new client
        var client = new Kubernetes(config);

        // create self subject rules review request
        var result = await K8sGuard.DoAsync(() => client.ListNodeAsync());
        
        // return nodes count
        return result.Items.Count;
    }
}