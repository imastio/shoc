using Shoc.ApiCore.GrpcClient;
using Shoc.Core;

namespace Shoc.Job.Services;

/// <summary>
/// The secret validation service
/// </summary>
public class GitRepoValidationService : ValidationServiceBase
{
    /// <summary>
    /// The maximum length of name
    /// </summary>
    private const int MAX_NAME_LENGTH = 256;

    /// <summary>
    /// The maximum length of owner
    /// </summary>
    private const int MAX_OWNER_LENGTH = 256;

    /// <summary>
    /// The maximum length of source
    /// </summary>
    private const int MAX_SOURCE_LENGTH = 256;

    /// <summary>
    /// The maximum repository length
    /// </summary>
    private const int MAX_REPOSITORY_LENGTH = 512;

    /// <summary>
    /// The maximum remote url length
    /// </summary>
    private const int MAX_REMOTE_URL_LENGTH = 768;
    
    /// <summary>
    /// Create a new service
    /// </summary>
    /// <param name="grpcClientProvider">The grpc client provider</param>
    public GitRepoValidationService(IGrpcClientProvider grpcClientProvider) : base(grpcClientProvider)
    {
    }
    
    /// <summary>
    /// Validate the name of the repo
    /// </summary>
    /// <param name="value">The value to validate</param>
    public void ValidateName(string value)
    {
        // check if empty or invalid
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_OWNER_LENGTH)
        {
            throw ErrorDefinition.Validation().AsException();
        }
    }
    
    /// <summary>
    /// Validate the owner of the repo
    /// </summary>
    /// <param name="value">The value to validate</param>
    public void ValidateOwner(string value)
    {
        // check if empty or invalid
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_NAME_LENGTH)
        {
            throw ErrorDefinition.Validation().AsException();
        }
    }
    
    /// <summary>
    /// Validate the source of the repo
    /// </summary>
    /// <param name="value">The value to validate</param>
    public void ValidateSource(string value)
    {
        // check if empty or invalid
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_SOURCE_LENGTH)
        {
            throw ErrorDefinition.Validation().AsException();
        }
    }
    
    /// <summary>
    /// Validate the repository name of the repo
    /// </summary>
    /// <param name="value">The value to validate</param>
    public void ValidateRepository(string value)
    {
        // check if empty or invalid
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_REPOSITORY_LENGTH)
        {
            throw ErrorDefinition.Validation().AsException();
        }
    }
    
    /// <summary>
    /// Validate the remote url of the repo
    /// </summary>
    /// <param name="value">The value to validate</param>
    public void ValidateRemoteUrl(string value)
    {
        // check if empty or invalid
        if (string.IsNullOrWhiteSpace(value) || value.Length > MAX_REMOTE_URL_LENGTH)
        {
            throw ErrorDefinition.Validation().AsException();
        }
    }
}