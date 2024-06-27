using System.Collections.Generic;

namespace Shoc.Core;

/// <summary>
/// The model for error definition
/// </summary>
public class ErrorDefinition
{
    /// <summary>
    /// The error kind
    /// </summary>
    public string Kind { get; set; }

    /// <summary>
    /// The code of error
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// The error message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// The error payload
    /// </summary>
    public Dictionary<string, object> Payload { get; set; }

    /// <summary>
    /// Throw the error within exception
    /// </summary>
    /// <param name="message">The extra message</param>
    public void Throw(string message = null)
    {
        throw this.AsException(message);
    }

    /// <summary>
    /// Returns the exception for the error
    /// </summary>
    /// <returns></returns>
    public ShocException AsException(string message = null)
    {
        return new ShocException([this], message);
    }
    
    /// <summary>
    /// Shortcut for creating unknown error
    /// </summary>
    /// <param name="code">The code of error</param>
    /// <param name="message">The error message</param>
    /// <param name="payload">The payload</param>
    /// <returns></returns>
    public static ErrorDefinition Unknown(string code = null, string message = null, Dictionary<string, object> payload = null)
    {
        return new ErrorDefinition
        {
            Kind = ErrorKinds.UNKNOWN,
            Code = code ?? Errors.UNKNOWN_ERROR,
            Message = message ?? string.Empty,
            Payload = payload
        };
    }

    /// <summary>
    /// Shortcut for creating unknown error
    /// </summary>
    /// <param name="kind">The kind of error</param>
    /// <param name="code">The code of error</param>
    /// <param name="message">The error message</param>
    /// <param name="payload">The payload</param>
    /// <returns></returns>
    public static ErrorDefinition From(string kind, string code, string message, Dictionary<string, object> payload = null)
    {
        return new ErrorDefinition
        {
            Kind = kind,
            Code = code ?? Errors.UNKNOWN_ERROR,
            Message = message ?? string.Empty,
            Payload = payload 
        };
    }

    /// <summary>
    /// Shortcut for creating data error
    /// </summary>
    /// <param name="code">The code of error</param>
    /// <param name="message">The error message</param>
    /// <param name="payload">The payload</param>
    /// <returns></returns>
    public static ErrorDefinition Data(string code = null, string message = null, Dictionary<string, object> payload = null)
    {
        return new ErrorDefinition
        {
            Kind = ErrorKinds.DATA,
            Code = code ?? Errors.DATA_ERROR,
            Message = message ?? string.Empty,
            Payload = payload
        };
    }

    /// <summary>
    /// Shortcut for creating not found error
    /// </summary>
    /// <param name="code">The code of error</param>
    /// <param name="message">The error message</param>
    /// <param name="payload">The payload</param>
    /// <returns></returns>
    public static ErrorDefinition NotFound(string code = null, string message = null, Dictionary<string, object> payload = null)
    {
        return new ErrorDefinition
        {
            Kind = ErrorKinds.NOT_FOUND,
            Code = code ?? Errors.NOT_FOUND_ERROR,
            Message = message ?? string.Empty,
            Payload = payload
        };
    }

    /// <summary>
    /// Shortcut for creating validation error
    /// </summary>
    /// <param name="code">The code of error</param>
    /// <param name="message">The error message</param>
    /// <param name="payload">The payload</param>
    /// <returns></returns>
    public static ErrorDefinition Validation(string code = null, string message = null, Dictionary<string, object> payload = null)
    {
        return new ErrorDefinition
        {
            Kind = ErrorKinds.VALIDATION,
            Code = code ?? Errors.VALIDATION_ERROR,
            Message = message ?? string.Empty,
            Payload = payload
        };
    }
    
    /// <summary>
    /// Shortcut for creating access error
    /// </summary>
    /// <param name="code">The code of error</param>
    /// <param name="message">The error message</param>
    /// <param name="payload">The payload</param>
    /// <returns></returns>
    public static ErrorDefinition Access(string code = null, string message = null, Dictionary<string, object> payload = null)
    {
        return new ErrorDefinition
        {
            Kind = ErrorKinds.ACCESS_DENIED,
            Code = code ?? Errors.ACCESS_ERROR,
            Message = message ?? string.Empty,
            Payload = payload
        };
    }
    
    /// <summary>
    /// Shortcut for creating access error
    /// </summary>
    /// <param name="code">The code of error</param>
    /// <param name="message">The error message</param>
    /// <param name="payload">The payload</param>
    /// <returns></returns>
    public static ErrorDefinition Authentication(string code = null, string message = null, Dictionary<string, object> payload = null)
    {
        return new ErrorDefinition
        {
            Kind = ErrorKinds.NOT_AUTHENTICATED,
            Code = code ?? Errors.ACCESS_ERROR,
            Message = message ?? string.Empty,
            Payload = payload
        };
    }
}
