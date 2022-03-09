using System.Collections.Generic;

namespace Shoc.Core
{
    /// <summary>
    /// The model for error definition
    /// </summary>
    public class ErrorDefinition
    {
        /// <summary>
        /// The error kind
        /// </summary>
        public ErrorKind Kind { get; set; }

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
            throw this.AsException();
        }

        /// <summary>
        /// Returns the exception of the error
        /// </summary>
        /// <returns></returns>
        public ShocException AsException()
        {
            return new ShocException(this);
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
                Kind = ErrorKind.Unknown,
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
        public static ErrorDefinition From(ErrorKind kind, string code, string message, Dictionary<string, object> payload = null)
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
                Kind = ErrorKind.Data,
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
                Kind = ErrorKind.NotFound,
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
                Kind = ErrorKind.Validation,
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
                Kind = ErrorKind.Access,
                Code = code ?? Errors.ACCESS_ERROR,
                Message = message ?? string.Empty,
                Payload = payload
            };
        }
    }
}