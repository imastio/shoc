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
    public static AggregateErrorDefinition Of(string kind, string message = null)
    {
        var error = kind switch
        {
            ErrorKinds.DATA => ErrorDefinition.Data(Core.Errors.DATA_ERROR, message),
            ErrorKinds.NOT_FOUND => ErrorDefinition.NotFound(Core.Errors.NOT_FOUND_ERROR, message),
            ErrorKinds.VALIDATION => ErrorDefinition.Validation(Core.Errors.VALIDATION_ERROR, message),
            ErrorKinds.ACCESS_DENIED => ErrorDefinition.Access(Core.Errors.ACCESS_ERROR, message),
            ErrorKinds.NOT_AUTHENTICATED => ErrorDefinition.Authentication(Core.Errors.AUTHENTICATION_ERROR, message),
            _ => ErrorDefinition.Unknown(Core.Errors.UNKNOWN_ERROR, message)
        };

        return new AggregateErrorDefinition
        {
            Errors = [error]
        };
    }
}
