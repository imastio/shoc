using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shoc.ApiCore;

/// <summary>
/// The attribute to disable FromValue model binding
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
{
    /// <summary>
    /// On resource executing handler to remove type provider factories
    /// </summary>
    /// <param name="context">The executing context</param>
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var factories = context.ValueProviderFactories;
        factories.RemoveType<FormValueProviderFactory>();
        factories.RemoveType<FormFileValueProviderFactory>();
        factories.RemoveType<JQueryFormValueProviderFactory>();
    }

    /// <summary>
    /// The resource executed handler
    /// </summary>
    /// <param name="context">The resource executed context</param>
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }
}