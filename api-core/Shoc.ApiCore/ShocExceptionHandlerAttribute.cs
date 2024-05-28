using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shoc.Core;

namespace Shoc.ApiCore;

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

        // cast to specific exception
        var specificException = exception as ShocException;

        // unknown error
        var errors = new List<ErrorDefinition> { ErrorDefinition.Unknown(Errors.UNKNOWN_ERROR, exception.Message) };

        // if specific exception consider getting internal errors
        if (specificException != null)
        {
            errors = specificException.Errors;
        }

        // the error kind
        var errorKind = specificException?.Errors?.FirstOrDefault()?.Kind;

        // deduce the code
        var statusCode = errorKind switch
        {
            ErrorKind.Unknown => 400,
            ErrorKind.Data => 404,
            ErrorKind.NotFound => 404,
            ErrorKind.Validation => 400,
            ErrorKind.Access => 403,
            null => 500,
            _ => 500
        };

        // build a result
        var result = new JsonResult(new AggregateErrorDefinition { Errors = errors })
        {
            StatusCode = statusCode
        };

        // return in the context
        context.Result = result;

        // mark error type
        context.HttpContext.Response.Headers["X-Shoc-Error"] = "aggregate";
    }
}
