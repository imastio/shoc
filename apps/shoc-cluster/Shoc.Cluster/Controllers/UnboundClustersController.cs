using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shoc.ApiCore;
using Shoc.ApiCore.Access;
using Shoc.Cluster.Model;
using Shoc.Cluster.Model.Cluster;
using Shoc.Cluster.Services;

namespace Shoc.Cluster.Controllers;

/// <summary>
/// The clusters endpoint
/// </summary>
[Route("api/unbound-clusters")]
[ApiController]
[ShocExceptionHandler]
public class UnboundClustersController : ControllerBase
{
    /// <summary>
    /// The cluster service
    /// </summary>
    private readonly ClusterService clusterService;

    /// <summary>
    /// Creates new instance of controller
    /// </summary>
    /// <param name="clusterService">The reference to service</param>
    public UnboundClustersController(ClusterService clusterService)
    {
        this.clusterService = clusterService;
    }

    /// <summary>
    /// Gets all the extended objects
    /// </summary>
    /// <returns></returns>
    [AuthorizeAnyAccess(ClusterAccesses.CLUSTER_CLUSTERS_LIST)]
    [HttpGet("extended")]
    public Task<IEnumerable<ClusterExtendedModel>> GetAllExtended()
    {
        return this.clusterService.GetAllExtended();
    }
}

