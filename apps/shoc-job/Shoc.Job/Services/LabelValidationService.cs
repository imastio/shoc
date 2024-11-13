using System.Text.RegularExpressions;
using Shoc.ApiCore.GrpcClient;
using Shoc.Core;
using Shoc.Job.Model;

namespace Shoc.Job.Services;

/// <summary>
/// The secret validation service
/// </summary>
public class LabelValidationService : ValidationServiceBase
{
    /// <summary>
    /// The allowed pattern of label names
    /// </summary>
    private static readonly Regex NAME_PATTERN = new("^[a-zA-Z][a-zA-Z0-9_-]{1,49}$");
    
    /// <summary>
    /// Create a new service
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public LabelValidationService(IGrpcClientProvider grpcClientProvider) : base(grpcClientProvider)
    {
    }
    
    /// <summary>
    /// Validate the name of the label
    /// </summary>
    /// <param name="name">The name to validate</param>
    public void ValidateName(string name)
    {
        // check if empty or not matching the pattern
        if (!IsNameValid(name))
        {
            throw ErrorDefinition.Validation(JobErrors.INVALID_LABEL_NAME).AsException();
        }
    }

    /// <summary>
    /// Check if the label name is valid
    /// </summary>
    /// <param name="name">The name to validate</param>
    /// <returns></returns>
    public static bool IsNameValid(string name)
    {
        // check if empty or not matching the pattern
        return !string.IsNullOrWhiteSpace(name) && NAME_PATTERN.IsMatch(name);
    }
}