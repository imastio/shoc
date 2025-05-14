using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Shoc.ApiCore.GrpcClient;
using Shoc.Cluster.Data;
using Shoc.Cluster.Model.Cluster;
using Shoc.Core.Kubernetes;

namespace Shoc.Cluster.Services;

/// <summary>
/// The cluster instance service
/// </summary>
public class ClusterInstanceService : ClusterServiceBase
{
    /// <summary>
    /// The kubernetes service
    /// </summary>
    private readonly K8sService k8SService;

    /// <summary>
    /// Creates new instance of registry service
    /// </summary>
    /// <param name="registryRepository">The registry repository</param>
    /// <param name="configurationProtectionProvider">The configuration protection provider</param>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    /// <param name="k8sService">The k8s service</param>
    public ClusterInstanceService(IClusterRepository registryRepository, ConfigurationProtectionProvider configurationProtectionProvider, IGrpcClientProvider grpcClientProvider, K8sService k8sService) : 
        base(registryRepository, configurationProtectionProvider, grpcClientProvider)
    {
        this.k8SService = k8sService;
    }

    /// <summary>
    /// Perform a dry test of configuration for a new object
    /// </summary>
    /// <returns></returns>
    public async Task<ClusterConnectionTestedModel> Ping(ClusterConnectionTestModel input)
    {
        // perform the test and return the result
        var result = await this.k8SService.GetNodesCount(new KubeContext{ Config = input.Configuration});
        
        // return existing object
        return new ClusterConnectionTestedModel
        {
            NodesCount = result
        };
    }
    
    /// <summary>
    /// Gets the connectivity info by id
    /// </summary>
    /// <param name="workspaceId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ClusterConnectivityModel> GetConnectivityById(string workspaceId, string id)
    {
        // ensure object exists
        var result = await this.RequireClusterById(workspaceId, id);

        // the protector instance
        var protector = this.configurationProtectionProvider.Create();

        // indicate if cluster is connected
        var connected = true;
        
        // connectivity message
        var message = $"Successfully connected to cluster {result.Name}";

        try
        {
            // do a basic operation on the cluster
            await this.k8SService.GetNodesCount(new KubeContext { Config = protector.Unprotect(result.Configuration) });
        }
        catch (Exception e)
        {
            // not connected
            connected = false;
            
            // error message
            message = e.Message;
        }

        // return the result
        return new ClusterConnectivityModel
        {
            Id = result.Id,
            WorkspaceId = result.WorkspaceId,
            Connected = connected,
            Message = message,
            Updated = DateTime.Now
        };
    }
}