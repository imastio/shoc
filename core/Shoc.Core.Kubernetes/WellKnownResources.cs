namespace Shoc.Core.Kubernetes;

/// <summary>
/// The well-known resources
/// </summary>
public static class WellKnownResources
{
    /// <summary>
    /// The gpu resource key
    /// </summary>
    public const string CPU = "cpu";
    
    /// <summary>
    /// The memory resource key
    /// </summary>
    public const string MEMORY = "memory";

    /// <summary>
    /// The nvidia.com gpu resource key
    /// </summary>
    public const string NVIDIA_GPU = "nvidia.com/gpu";

    /// <summary>
    /// The amd.com gpu resource key
    /// </summary>
    public const string AMD_GPU = "amd.com/gpu";
}