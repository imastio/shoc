using System.Collections.Generic;

namespace Shoc.ApiCore.Intl;

/// <summary>
/// The Intl Settings
/// </summary>
public class IntlSettings
{
    /// <summary>
    /// The default locale
    /// </summary>
    public string DefaultLocale { get; set; }
    
    /// <summary>
    /// The set of supported locales
    /// </summary>
    public List<string> SupportedLocales { get; set; }
}
