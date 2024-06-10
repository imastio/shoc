using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Shoc.ApiCore.RazorEngine;

/// <summary>
/// The MVC-based razor engine implementation
/// </summary>
public class RazorEngineMvc : IRazorEngine
{
    /// <summary>
    /// The razor view engine
    /// </summary>
    private readonly IRazorViewEngine viewEngine;
    
    /// <summary>
    /// The temporary data provider
    /// </summary>
    private readonly ITempDataProvider tempDataProvider;
    
    /// <summary>
    /// The service provider
    /// </summary>
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Creates new instance of razor engine MVC
    /// </summary>
    /// <param name="viewEngine">The razor view engine</param>
    /// <param name="tempDataProvider">The temporary data provider</param>
    /// <param name="serviceProvider">The service provider</param>
    public RazorEngineMvc(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
    {
        this.viewEngine = viewEngine;
        this.tempDataProvider = tempDataProvider;
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Renders the given view with the model into a string
    /// </summary>
    /// <param name="view">The view name</param>
    /// <param name="model">The model</param>
    /// <typeparam name="TModel">The model type</typeparam>
    /// <returns>The rendered razor page string</returns>
    public async Task<string> Render<TModel>(string view, TModel model)
    {
        // create a scope
        using var scope = this.serviceProvider.CreateScope();
        
        // gets the action context
        var actionContext = this.GetActionContext(scope);
        
        // try find razor view with name and action context
        var razorView = this.FindView(actionContext, view);

        // a writer to keep the rendered data
        await using var output = new StringWriter();

        // create the view data object
        var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };

        // create temporary data
        var tempData = new TempDataDictionary(actionContext.HttpContext, this.tempDataProvider);
        
        // start rendering the view async
        await razorView.RenderAsync(new ViewContext(actionContext, razorView, viewData, tempData, output, new HtmlHelperOptions()));

        // return output of page
        return output.ToString();
    }
    
    /// <summary>
    /// Finds a view with given name and context
    /// </summary>
    /// <param name="actionContext">The action context></param>
    /// <param name="view">The name of view</param>
    /// <returns></returns>
    private IView FindView(ActionContext actionContext, string view)
    {
        // try get view with name as a main page
        var viewResult = this.viewEngine.GetView(null, view, true);
        
        // return view if found
        if (viewResult.Success)
        {
            return viewResult.View;
        }

        // try find the view with action context
        viewResult = this.viewEngine.FindView(actionContext, view, true);
        
        // return the view if found
        if (viewResult.Success)
        {
            return viewResult.View;
        }
        
        throw new InvalidOperationException("Could not find the requested view");
    }
    
    /// <summary>
    /// Gets a new action context
    /// </summary>
    /// <returns></returns>
    private ActionContext GetActionContext(IServiceScope scope)
    {
        // create a default http context with service provider 
        var httpContext = new DefaultHttpContext
        {
            RequestServices = scope.ServiceProvider
        };
        
        // build new action context 
        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }
}
