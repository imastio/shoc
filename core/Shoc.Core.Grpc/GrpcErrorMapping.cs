using System.Collections.Generic;
using System.Linq;
using Grpc.Core;

namespace Shoc.Core.Grpc;

/// <summary>
/// The Grpc error mapping
/// </summary>
public static class GrpcErrorMapping
{
    /// <summary>
    /// A mapping of well known errors to grpc alternatives
    /// </summary>
    private static readonly IDictionary<string, StatusCode> ERRORS_MAP = new Dictionary<string, StatusCode>
    {
        { ErrorKinds.UNKNOWN, StatusCode.Unknown },
        { ErrorKinds.DATA, StatusCode.DataLoss },
        { ErrorKinds.NOT_FOUND, StatusCode.NotFound },
        { ErrorKinds.VALIDATION, StatusCode.InvalidArgument },
        { ErrorKinds.ACCESS_DENIED, StatusCode.PermissionDenied },
        { ErrorKinds.NOT_AUTHENTICATED, StatusCode.Unauthenticated }
    };
    
    /// <summary>
    /// A mapping of grpc errors to the well known error kinds
    /// </summary>
    private static readonly IDictionary<StatusCode, string> REVERSE_MAP = ERRORS_MAP.ToDictionary(kv => kv.Value, kv => kv.Key);

    /// <summary>
    /// Try mapping the error kind to the status code
    /// </summary>
    /// <param name="errorKind">The error kind</param>
    /// <returns></returns>
    public static StatusCode ToStatusCode(string errorKind)
    {
        // an unknown err
        if (string.IsNullOrWhiteSpace(errorKind))
        {
            return StatusCode.Unknown;
        }

        return ERRORS_MAP.TryGetValue(errorKind, out var statusCode) ? statusCode : StatusCode.Unknown;
    }
    
    /// <summary>
    /// Try mapping the status code to the error kind
    /// </summary>
    /// <param name="statusCode">The status code</param>
    /// <returns></returns>
    public static string ToErrorKind(StatusCode statusCode)
    {
        return REVERSE_MAP.TryGetValue(statusCode, out var errorKind) ? errorKind : ErrorKinds.UNKNOWN;
    }

    /// <summary>
    /// Maps the error definition to the grpc error type
    /// </summary>
    /// <param name="error">The error to map</param>
    /// <returns></returns>
    public static GrpcErrorDefinition ToGrpcErrorDefinition(ErrorDefinition error)
    {
        var result =  new GrpcErrorDefinition
        {
            Kind = error.Kind,
            Code = error.Code,
            Message = error.Message
        };

        // map the payload
        var payload = error.Payload?.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());

        // add payload if any
        if (payload != null)
        {
            result.Payload.Add(payload);
        }

        return result;
    }
    
    /// <summary>
    /// Maps the grpc error definition to the well known error type
    /// </summary>
    /// <param name="error">The error to map</param>
    /// <returns></returns>
    public static ErrorDefinition FromGrpcErrorDefinition(GrpcErrorDefinition error)
    {
        return ErrorDefinition.From(error.Kind, error.Code, error.Message, error.Payload?.ToDictionary(kv => kv.Key, kv => (object)kv.Value));
    }
}