using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Shoc.ApiCore;

/// <summary>
/// The set of extensions to HTTP Context
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Turn query collection into dictionary of value or values
    /// </summary>
    /// <param name="query">The query collection</param>
    /// <returns></returns>
    public static IDictionary<string, object> ToValuesDictionary(this IQueryCollection query)
    {
        // the data to collect
        var data = new Dictionary<string, object>();

        // go over all the query attributes from authorization callback
        foreach (var (key, values) in query)
        {
            // the empty value 
            object value = string.Empty;

            // get first if only one value is given
            if (values.Count == 1)
            {
                value = values[0];
            }

            if (values.Count > 1)
            {
                value = values.ToList();
            }

            data[key] = value;
        }

        return data;
    }


    /// <summary>
    /// Gets the item with given key or default
    /// </summary>
    /// <param name="httpContext">The http context</param>
    /// <param name="key">The key of item</param>
    /// <param name="defaultValue">The default value</param>
    /// <returns></returns>
    public static TValue GetItemOrDefault<TValue>(this HttpContext httpContext, string key, TValue defaultValue) where TValue : class
    {
        return httpContext.Items.TryGetValue(key, out var itemResult) ? (TValue) itemResult : defaultValue;
    }
}
