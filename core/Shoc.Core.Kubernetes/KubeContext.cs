using System;
using System.IO;
using System.Text;
using k8s;

namespace Shoc.Core.Kubernetes;

/// <summary>
/// The Kubernetes invocation context
/// </summary>
public class KubeContext
{
    /// <summary>
    /// The configuration string
    /// </summary>
    public string Config { get; set; }

    /// <summary>
    /// Creates a new stream from the existing config text
    /// </summary>
    /// <returns></returns>
    public Stream AsStream()
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(this.Config ?? ""));
    }

    /// <summary>
    /// Creates a new client configuration from the config text
    /// </summary>
    /// <returns></returns>
    public KubernetesClientConfiguration AsClientConfiguration()
    {
        // build a stream for configuration
        using var stream = this.AsStream();

        try
        {
            // build a config file
            return KubernetesClientConfiguration.BuildConfigFromConfigFile(stream);
        }
        catch (Exception)
        {
            throw ErrorDefinition.Validation().AsException();
        }
    }
}