using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Shoc.ApiCore
{
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
    }
}