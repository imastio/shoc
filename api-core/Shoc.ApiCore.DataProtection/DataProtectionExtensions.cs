using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shoc.DataProtection;

namespace Shoc.ApiCore.DataProtection;

/// <summary>
/// The set of extensions for data protection
/// </summary>
public static class DataProtectionExtensions
{
    /// <summary>
    /// Add data protection to the pipeline
    /// </summary>
    /// <param name="services">The services collection</param>
    public static IDataProtectionBuilder AddPersistenceDataProtection(this IServiceCollection services)
    {
        return services.AddDataProtection().PersistKeysToDatabase().SetApplicationName("/app");
    }

    /// <summary>
    /// Add persistence of 
    /// </summary>
    /// <param name="builder">The <see cref="IDataProtectionBuilder"/>.</param>
    /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
    private static IDataProtectionBuilder PersistKeysToDatabase(this IDataProtectionBuilder builder)
    {
        // adding persistence store to the pipeline
        builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(sp =>
        {
            // get key repository
            var keyRepository = sp.GetRequiredService<IProtectionKeyRepository>();

            // configure key store with repository
            return new ConfigureOptions<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new ProtectionKeyStore(keyRepository);
            });
        });

        return builder;
    }
}
