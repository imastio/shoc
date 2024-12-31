using System.Collections.Generic;

namespace Shoc.Job.Model.JobTask;

/// <summary>
/// The definition of resources for task
/// </summary>
public class JobTaskResourcesModel
{
    /// <summary>
    /// The number of CPU units to allocate
    /// </summary>
    public long? Cpu { get; set; }
    
    /// <summary>
    /// The amount of memory to allocate
    /// </summary>
    public long? Memory { get; set; }
    
    /// <summary>
    /// The amount of NVIDIA GPU units to allocate
    /// </summary>
    public long? NvidiaGpu { get; set; }
    
    /// <summary>
    /// The amount of AMD GPU units to allocate
    /// </summary>
    public long? AmdGpu { get; set; }
}