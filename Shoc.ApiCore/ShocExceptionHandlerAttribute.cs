using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shoc.Core;

namespace Shoc.ApiCore
{
    /// <summary>
    /// Data validation attribute
    /// </summary>
    public class ShocExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Handle data validation type exceptions
        /// </summary>
        /// <param name="context">The exception context</param>
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            // cast to shoc exception
            var shocException = exception as ShocException;

            // unknown error
            var errors = new List<ErrorDefinition> { ErrorDefinition.Unknown(Errors.UNKNOWN_ERROR) };
            
            // if shoc exception consider getting internal errors
            if (shocException != null)
            {
                errors = shocException.Errors;
            }
            
            // build a result
            var result = new JsonResult(new AggregateErrorDefinition { Errors = errors })
            {
                StatusCode = shocException == null ? 500 : 400
            };
                
            // return in the context
            context.Result = result;

            // mark error type
            context.HttpContext.Response.Headers["X-Shoc-Error"] = "aggregate";
        }
    }
}