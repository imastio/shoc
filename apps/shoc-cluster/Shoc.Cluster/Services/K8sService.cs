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
    /// The resource parser 
    /// </summary>
    private readonly ResourceParser resourceParser;

    /// <summary>
    /// Creates new intance of K8s service
    /// </summary>
    /// <param name="resourceParser">The resource parser</param>
    public K8sService(ResourceParser resourceParser)
    {
        this.resourceParser = resourceParser;
    }

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
    
    // public async Task<object> GetClusterSummary(KubeContext context)
    // {
    //     // Load from the default kubeconfig on the machine.
    //     var config = context.AsClientConfiguration();
    //
    //     // creates a new client
    //     var client = new Kubernetes(config);
    //     
    //     // Get nodes to calculate total resources
    //     var nodes = await K8sGuard.DoAsync(() => client.CoreV1.ListNodeAsync());
    //     
    //     // Get pods across all namespaces to calculate used resources
    //     var pods = await K8sGuard.DoAsync(() => client.CoreV1.ListPodForAllNamespacesAsync());
    //     
    //     var summary = new
    //     {
    //         NodeCount = nodes.Items.Count,
    //         TotalCpu = nodes.Items.Sum(n => 
    //                 Convert.ToDouble(n.Status.Capacity["cpu"].ToString().Replace("m", "")) / 1000,
    //             TotalMemory = nodes.Items.Sum(n => 
    //                 ParseKubernetesMemory(n.Status.Capacity["memory"].ToString())),
    //             TotalGpu = nodes.Items.Sum(n => 
    //                 n.Status.Capacity.ContainsKey("nvidia.com/gpu") ? 
    //                     int.Parse(n.Status.Capacity["nvidia.com/gpu"].ToString()) : 0),
    //             UsedCpu = pods.Items.Sum(p => 
    //                 p.Spec.Containers.Sum(c => 
    //                     c.Resources?.Requests?.ContainsKey("cpu") == true ? 
    //                         Convert.ToDouble(c.Resources.Requests["cpu"].ToString().Replace("m", "")) / 1000 : 0)),
    //             UsedMemory = pods.Items.Sum(p => 
    //                 p.Spec.Containers.Sum(c => 
    //                     c.Resources?.Requests?.ContainsKey("memory") == true ? 
    //                         ParseKubernetesMemory(c.Resources.Requests["memory"].ToString()) : 0)),
    //             UsedGpu = pods.Items.Sum(p => 
    //                 p.Spec.Containers.Sum(c => 
    //                     c.Resources?.Requests?.ContainsKey("nvidia.com/gpu") == true ? 
    //                         int.Parse(c.Resources.Requests["nvidia.com/gpu"].ToString()) : 0)),
    //             HealthyNodes = nodes.Items.Count(n => 
    //                 n.Status.Conditions.Any(c => 
    //                     c.Type == "Ready" && c.Status == "True")),
    //             UnhealthyNodes = nodes.Items.Count(n => 
    //                 n.Status.Conditions.Any(c => 
    //                     c.Type == "Ready" && c.Status != "True"))
    //     };
    //     
    //     try
    //     {
    //         
    //         
    //         
    //         
    //         // Calculate total and used resources
    //         
    //
    //         return Ok(summary);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"Error retrieving cluster summary: {ex.Message}");
    //     }
    // }

    /// <summary>
    /// Get node CPU
    /// </summary>
    /// <param name="node">The node reference</param>
    /// <returns></returns>
    private long GetNodeCpu(V1Node node)
    {
        // get the node resources 
        var resource = node.Status.Capacity.TryGetValue(WellKnownResources.CPU, out var qty) ? qty : null;
        
        // return parsed resource
        return this.resourceParser.ParseToMillicores(resource?.ToString()) ?? 0;
    }
    
    /// <summary>
    /// Get node memory
    /// </summary>
    /// <param name="node">The node reference</param>
    /// <returns></returns>
    private long GetNodeMemory(V1Node node)
    {
        // get the node resources 
        var resource = node.Status.Capacity.TryGetValue(WellKnownResources.MEMORY, out var qty) ? qty : null;
        
        // return parsed resource
        return this.resourceParser.ParseToBytes(resource?.ToString()) ?? 0;
    }
    
    /// <summary>
    /// Get node nvidia gpu
    /// </summary>
    /// <param name="node">The node reference</param>
    /// <returns></returns>
    private long GetNodeNvidiaGpu(V1Node node)
    {
        // get the node resources 
        var resource = node.Status.Capacity.TryGetValue(WellKnownResources.MEMORY, out var qty) ? qty : null;
        
        // return parsed resource
        return this.resourceParser.ParseToBytes(resource?.ToString()) ?? 0;
    }
}