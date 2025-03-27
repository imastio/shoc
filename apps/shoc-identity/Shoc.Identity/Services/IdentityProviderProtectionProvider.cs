namespace Shoc.Identity.Services;

using Microsoft.AspNetCore.DataProtection;

/// <summary>
/// The protection provider
/// </summary>
public class IdentityProviderProtectionProvider
{
    /// <summary>
    /// The protection purpose
    /// </summary>
    private const string IDENTITY_PROVIDER_PROTECTION_PURPOSE = "idp-protector";

    /// <summary>
    /// The data protection provider
    /// </summary>
    private readonly IDataProtectionProvider dataProtectionProvider;

    /// <summary>
    /// Creates new protection provider
    /// </summary>
    /// <param name="dataProtectionProvider">The protection provider</param>
    public IdentityProviderProtectionProvider(IDataProtectionProvider dataProtectionProvider)
    {
        this.dataProtectionProvider = dataProtectionProvider;
    }

    /// <summary>
    /// Creates a new data protector
    /// </summary>
    /// <returns></returns>
    public IDataProtector Create()
    {
        return this.dataProtectionProvider.CreateProtector(IDENTITY_PROVIDER_PROTECTION_PURPOSE);
    }
}