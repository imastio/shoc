using System.Threading.Tasks;

namespace Shoc.ApiCore.RazorEngine;

/// <summary>
/// The razor engine interface to render razor pages as string
/// </summary>
public interface IRazorEngine
{
    /// <summary>
    /// Renders the given view with the model into a string
    /// </summary>
    /// <param name="view">The view name</param>
    /// <param name="model">The model</param>
    /// <typeparam name="TModel">The model type</typeparam>
    /// <returns>The rendered razor page string</returns>
    Task<string> Render<TModel>(string view, TModel model);
}
