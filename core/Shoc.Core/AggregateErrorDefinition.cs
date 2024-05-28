using System.Collections.Generic;

namespace Shoc.Core;

/// <summary>
/// The aggregated error definition
/// </summary>
public class AggregateErrorDefinition
{
    /// <summary>
    /// The underlying error definitions
    /// </summary>
    public List<ErrorDefinition> Errors { get; set; }

    /// <summary>
    /// Creates an aggregate error of the given kind
    /// </summary>
    /// <param name="kind">The kind</param>
    /// <param name="message">The message</param>
    /// <returns></returns>
    public static AggregateErrorDefinition Of(ErrorKind kind, string message = null)
    {
        var error = kind switch
        {
            ErrorKind.Data => ErrorDefinition.Unknown(Core.Errors.DATA_ERROR, message),
            ErrorKind.NotFound => ErrorDefinition.Unknown(Core.Errors.NOT_FOUND_ERROR, message),
            ErrorKind.Validation => ErrorDefinition.Unknown(Core.Errors.VALIDATION_ERROR, message),
            ErrorKind.Access => ErrorDefinition.Unknown(Core.Errors.ACCESS_ERROR, message),
            _ => ErrorDefinition.Unknown(Core.Errors.UNKNOWN_ERROR, message)
        };

        return new AggregateErrorDefinition
        {
            Errors = [error]
        };
    }
}
