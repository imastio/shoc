using System.Collections.Generic;

namespace Shoc.Cluster.Model.Cluster;

/// <summary>
/// The cluster node model
/// </summary>
public class ClusterNodeModel
{
    /// <summary>
    /// The node name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The node labels
    /// </summary>
    public IDictionary<string, string> Labels { get; set; } 
    
    /// <summary>
    /// The allocatable CPU
    /// </summary>
    public long AllocatableCpu { get; set; }
    
    /// <summary>
    /// The allocatable memory
    /// </summary>
    public long AllocatableMemory { get; set; }
    
    /// <summary>
    /// The allocatable Nvidia GPU
    /// </summary>
    public long AllocatableNvidiaGpu { get; set; }
    
    /// <summary>
    /// The allocatable AMD GPU
    /// </summary>
    public long AllocatableAmdGpu { get; set; }
    
    /// <summary>
    /// The allocatable CPU
    /// </summary>
    public long CapacityCpu { get; set; }
    
    /// <summary>
    /// The allocatable memory
    /// </summary>
    public long CapacityMemory { get; set; }
    
    /// <summary>
    /// The allocatable Nvidia GPU
    /// </summary>
    public long CapacityNvidiaGpu { get; set; }
    
    /// <summary>
    /// The allocatable AMD GPU
    /// </summary>
    public long CapacityAmdGpu { get; set; }
    
    /// <summary>
    /// The allocatable CPU
    /// </summary>
    public long? UsedCpu { get; set; }
    
    /// <summary>
    /// The allocatable memory
    /// </summary>
    public long? UsedMemory { get; set; }
}