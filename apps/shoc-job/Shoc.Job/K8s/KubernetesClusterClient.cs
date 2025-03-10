using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using Shoc.Core.Kubernetes;
using Shoc.Job.K8s.Model;

namespace Shoc.Job.K8s;

/// <summary>
/// The kubernetes cluster client
/// </summary>
public class KubernetesClusterClient : KubernetesClientBase, IDisposable
{
    /// <summary>
    /// The kubernetes client for job operations
    /// </summary>
    /// <param name="config">The cluster config for authentication</param>
    public KubernetesClusterClient(string config) : base(config)
    {
    }
    
    /// <summary>
    /// Gets the node resources
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<NodeResourceResult>> GetNodeResources()
    {
        // list nodes
        var nodes = await client.ListNodeAsync();

        // the result collection
        return nodes.Items.Select(node => new NodeResourceResult
        {
            Name = node.Metadata.Name,
            Cpu = node.Status.Allocatable[WellKnownResources.CPU].ToString(),
            Memory = node.Status.Allocatable[WellKnownResources.MEMORY].ToString(),
            NvidiaGpu = node.Status.Allocatable.TryGetValue(WellKnownResources.NVIDIA_GPU, out var nvidiaValue) ? nvidiaValue.ToString() : "0",
            AmdGpu = node.Status.Allocatable.TryGetValue(WellKnownResources.AMD_GPU, out var amdValue) ? amdValue.ToString() : "0"
        });
    }
    
    /// <summary>
    /// Disposes the client
    /// </summary>
    public void Dispose()
    {
        this.client?.Dispose();
        GC.SuppressFinalize(this);
    }
}