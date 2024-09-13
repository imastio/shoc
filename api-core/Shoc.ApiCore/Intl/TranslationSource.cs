using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Shoc.ApiCore.Intl;

/// <summary>
/// The translation source definition
/// </summary>
public class TranslationSource
{
    /// <summary>
    /// The locale of source
    /// </summary>
    public string Locale { get; set; }
    
    /// <summary>
    /// The locale aliases
    /// </summary>
    public List<string> Aliases { get; set; }
    
    /// <summary>
    /// The set of messages
    /// </summary>
    public IDictionary<string, string> Messages { get; set; }

    /// <summary>
    /// The path to target Json
    /// </summary>
    /// <param name="locale">The locale</param>
    /// <param name="aliases">The locale aliases</param>
    /// <param name="path">The full path of file</param>
    /// <returns></returns>
    public static TranslationSource FromJson(string locale, List<string> aliases, string path)
    {
        return new TranslationSource
        {
            Locale = locale,
            Aliases = aliases,
            Messages = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path))
        };
    }
}
