using System.Collections.Generic;
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
    public async Task<IList<V1Node>> GetNodes(KubeContext context)
    {
        // get the client configuration
        var config = context.AsClientConfiguration();

        // creates a new client
        var client = new Kubernetes(config);

        // create self subject rules review request
        var result = await K8sGuard.DoAsync(() => client.ListNodeAsync());

        // get the node items
        return result.Items;
    }

    public async Task<IDictionary<string, NodeMetrics>> GetNodeMetrics(KubeContext context)
    {
        // get the client configuration
        var config = context.AsClientConfiguration();

        // creates a new client
        var client = new Kubernetes(config);
        
        // load node metrics response
        var nodeMetricsResponse = await client.CustomObjects.ListClusterCustomObjectAsync<NodeMetricsList>(
            group: "metrics.k8s.io",
            version: "v1beta1",
            plural: "nodes"
        );

        // resulting collection
        var result = new Dictionary<string, NodeMetrics>();

        // turn into dictionary
        foreach (var nodeMetric in nodeMetricsResponse.Items)
        {
            // no info about unnamed node
            if (string.IsNullOrWhiteSpace(nodeMetric.Name()))
            {
                continue;
            }

            // record by name
            result[nodeMetric.Name()] = nodeMetric;
        }

        // return result
        return result;
    }
}
