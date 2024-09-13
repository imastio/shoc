using System.Collections.Generic;

namespace Shoc.ApiCore.Intl;

/// <summary>
/// The Intl Service interface
/// </summary>
public interface IIntlService
{
    /// <summary>
    /// Format the message based on message id and arguments
    /// </summary>
    /// <param name="id">The id of message</param>
    /// <param name="locale">The target locale</param>
    /// <param name="fallback">The fallback message</param>
    /// <param name="args">The arguments</param>
    /// <returns></returns>
    string Format(string id, string locale, string fallback, IDictionary<string, object> args = null);

    /// <summary>
    /// Format the message based on message id and arguments
    /// </summary>
    /// <param name="id">The id of message</param>
    /// <param name="locale">The target locale</param>
    /// <param name="args">The arguments</param>
    /// <returns></returns>
    string Format(string id, string locale, IDictionary<string, object> args = null);

    /// <summary>
    /// Gets the default locale
    /// </summary>
    /// <returns></returns>
    string GetDefaultLocale();
}
