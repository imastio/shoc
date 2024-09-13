using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;
using System.Reflection;
using Shoc.ApiCore;

namespace Shoc.ApiCore.Intl;

/// <summary>
/// The intl service extensions
/// </summary>
public static class IntlExtensions
{
    /// <summary>
    /// Configures Intl essentials
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <param name="sources">The sources of translation</param>
    /// <returns></returns>
    public static IServiceCollection AddIntlEssentials(this IServiceCollection services, IConfiguration configuration, params TranslationSource[] sources)
    {
        // register self settings
        services.AddSingleton(configuration.BindAs<IntlSettings>("Intl"));

        // add new translation sources
        var translationStore = new InMemoryTranslationStore(sources);

        // register as primary translation store
        services.AddSingleton<ITranslationStore>(translationStore);

        // add Intl service
        services.AddSingleton<IIntlService, IntlService>();

        // continue chaining
        return services;
    }

    /// <summary>
    /// Build a source from local path
    /// </summary>
    /// <param name="locale">The locale</param>
    /// <param name="aliases">The aliases</param>
    /// <param name="localPath">The locale path</param>
    /// <returns></returns>
    public static TranslationSource FromJson(string locale, string[] aliases, string localPath)
    {
        // source directory
        var sourceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        // build full path to key
        var jsonPath = Path.GetFullPath(localPath, sourceDirectory);

        // build a json locale source
        return TranslationSource.FromJson(locale, aliases.ToList(), jsonPath);
    }
}
