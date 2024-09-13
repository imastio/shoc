using System.Collections.Generic;
using System.Linq;

namespace Shoc.ApiCore.Intl;

/// <summary>
/// An in-memory translation store
/// </summary>
public class InMemoryTranslationStore : ITranslationStore
{
    /// <summary>
    /// The set of all messages by (locale, id) pair
    /// </summary>
    private readonly IDictionary<string, IDictionary<string, string>> messages;

    /// <summary>
    /// Creates new instance of in-memory translation store
    /// </summary>
    /// <param name="sources">The translation sources</param>
    public InMemoryTranslationStore(params TranslationSource[] sources)
    {
        this.messages = new Dictionary<string, IDictionary<string, string>>();

        // add each source
        foreach (var source in sources)
        {
            // add messages by locale
            this.messages[source.Locale] = source.Messages;

            // add each locale alias to refer same set of messages
            foreach (var alias in source.Aliases ?? Enumerable.Empty<string>())
            {
                this.messages[alias] = source.Messages;
            }
        }
    }

    /// <summary>
    /// Gets the message template by id and locale
    /// </summary>
    /// <param name="id">The message id</param>
    /// <param name="locale">The locale name</param>
    /// <returns></returns>
    public string Get(string id, string locale)
    {
        // the locale is not defined at all
        if (!this.messages.TryGetValue(locale, out var templates))
        {
            return null;
        }

        // get if defined by id
        return templates.TryGetValue(id, out var template) ? template : null;
    }
}
