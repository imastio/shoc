using System.Collections.Generic;
using System.Linq;

namespace Shoc.ApiCore.Intl;

/// <summary>
/// The Intl Service implementation
/// </summary>
public class IntlService : IIntlService
{
    /// <summary>
    /// The intl settings
    /// </summary>
    private readonly IntlSettings settings;

    /// <summary>
    /// The translation store
    /// </summary>
    private readonly ITranslationStore store;

    /// <summary>
    /// Creates new instance of intl service
    /// </summary>
    /// <param name="settings">The settings</param>
    /// <param name="store">The translation store</param>
    public IntlService(IntlSettings settings, ITranslationStore store)
    {
        this.settings = settings;
        this.store = store;
    }

    /// <summary>
    /// Format the message based on message id and arguments
    /// </summary>
    /// <param name="id">The id of message</param>
    /// <param name="locale">The target locale</param>
    /// <param name="fallback">The fallback message</param>
    /// <param name="args">The arguments</param>
    /// <returns></returns>
    public string Format(string id, string locale, string fallback, IDictionary<string, object> args = null)
    {
        // try get by id and locale or by fallback locale
        var template = this.store.Get(id, locale) ?? this.store.Get(id, this.settings.DefaultLocale);

        // use id as fallback if no string is defined
        if (template == null)
        {
            return id;
        }
        
        // no arguments to replace
        if(args == null || args.Count == 0)
        {
            return template;
        }

        // process each argument in the string
        return args.Aggregate(template, (current, arg) => current.Replace($"{{{arg.Key}}}", arg.Value?.ToString() ?? string.Empty));
    }
    
    /// <summary>
    /// Format the message based on message id and arguments
    /// </summary>
    /// <param name="id">The id of message</param>
    /// <param name="locale">The target locale</param>
    /// <param name="args">The arguments</param>
    /// <returns></returns>
    public string Format(string id, string locale, IDictionary<string, object> args = null)
    {
        return this.Format(id, locale, string.Empty, args);
    }

    /// <summary>
    /// Gets the default locale
    /// </summary>
    /// <returns></returns>
    public string GetDefaultLocale()
    {
        return this.settings.DefaultLocale;
    }
}
