using Microsoft.AspNetCore.DataProtection;

namespace Shoc.Cluster.Services;

/// <summary>
/// The configuration protection provider
/// </summary>
public class ConfigurationProtectionProvider
{
    /// <summary>
    /// The cluster configuration purpose
    /// </summary>
    private const string CLUSTER_CONFIGURATION_PURPOSE = "cluster_configuration";

    /// <summary>
    /// The data protection provider
    /// </summary>
    private readonly IDataProtectionProvider dataProtectionProvider;

    /// <summary>
    /// Creates new cluster configuration protection provider
    /// </summary>
    /// <param name="dataProtectionProvider">The protection provider</param>
    public ConfigurationProtectionProvider(IDataProtectionProvider dataProtectionProvider)
    {
        this.dataProtectionProvider = dataProtectionProvider;
    }

    /// <summary>
    /// Creates a new data protector
    /// </summary>
    /// <returns></returns>
    public IDataProtector Create()
    {
        return this.dataProtectionProvider.CreateProtector(CLUSTER_CONFIGURATION_PURPOSE);
    }
}