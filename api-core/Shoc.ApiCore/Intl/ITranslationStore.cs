namespace Shoc.ApiCore.Intl;

/// <summary>
/// The translation store
/// </summary>
public interface ITranslationStore
{
    /// <summary>
    /// Gets the translation by id and locale 
    /// </summary>
    /// <param name="id">The id of translation string</param>
    /// <param name="locale">The target locale</param>
    /// <returns></returns>
    string Get(string id, string locale);
}
