using Microsoft.AspNetCore.DataProtection;

namespace Shoc.Job.Services;

/// <summary>
/// The protection provider
/// </summary>
public class JobProtectionProvider
{
    /// <summary>
    /// The protection purpose
    /// </summary>
    private const string JOB_DATA_PURPOSE = "job-data";

    /// <summary>
    /// The data protection provider
    /// </summary>
    private readonly IDataProtectionProvider dataProtectionProvider;

    /// <summary>
    /// Creates new protection provider
    /// </summary>
    /// <param name="dataProtectionProvider">The protection provider</param>
    public JobProtectionProvider(IDataProtectionProvider dataProtectionProvider)
    {
        this.dataProtectionProvider = dataProtectionProvider;
    }

    /// <summary>
    /// Creates a new data protector
    /// </summary>
    /// <returns></returns>
    public IDataProtector Create()
    {
        return this.dataProtectionProvider.CreateProtector(JOB_DATA_PURPOSE);
    }
}