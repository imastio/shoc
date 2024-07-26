namespace Shoc.Core.Discovery;

/// <summary>
/// The known hosting
/// </summary>
public class HostingTypes
{
    /// <summary>
    /// The localhost indicates services are hosted on the local machine
    /// </summary>
    public const string LOCALHOST = "localhost";

    /// <summary>
    /// The docker indicates services are hosted inside a docker containers
    /// </summary>
    public const string DOCKER = "docker";

    /// <summary>
    /// The k8s indicates services are hosted inside a Kubernetes cluster
    /// </summary>
    public const string K8S = "k8s";
}