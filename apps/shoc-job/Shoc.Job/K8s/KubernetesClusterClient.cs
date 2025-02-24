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
public class KubernetesClusterClient : IDisposable
{
    /// <summary>
    /// The underlying kubernetes client
    /// </summary>
    private readonly Kubernetes client;

    /// <summary>
    /// The kubernetes client for job operations
    /// </summary>
    /// <param name="config">The cluster config for authentication</param>
    public KubernetesClusterClient(string config)
    {
        this.client = new Kubernetes(new KubeContext { Config = config }.AsClientConfiguration());
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
            Cpu = node.Status.Allocatable["cpu"].ToString(),
            Memory = node.Status.Allocatable["memory"].ToString(),
            NvidiaGpu = node.Status.Allocatable.TryGetValue("nvidia.com/gpu", out var nvidiaValue) ? nvidiaValue.ToString() : "0",
            AmdGpu = node.Status.Allocatable.TryGetValue("amd.com/gpu", out var amdValue) ? amdValue.ToString() : "0"
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