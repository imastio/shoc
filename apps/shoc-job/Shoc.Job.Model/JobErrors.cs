namespace Shoc.Job.Model;

/// <summary>
/// The job errors
/// </summary>
public static class JobErrors
{   
    /// <summary>
    /// The workspace is invalid
    /// </summary>
    public const string INVALID_WORKSPACE = "JOB_INVALID_WORKSPACE";
    
    /// <summary>
    /// The user is invalid
    /// </summary>
    public const string INVALID_USER = "JOB_INVALID_USER";
    
    /// <summary>
    /// The package is invalid
    /// </summary>
    public const string INVALID_PACKAGE = "JOB_INVALID_PACKAGE";

    /// <summary>
    /// The registry is invalid
    /// </summary>
    public const string INVALID_REGISTRY = "JOB_INVALID_REGISTRY";
    
    /// <summary>
    /// The label name is invalid
    /// </summary>
    public const string INVALID_LABEL_NAME = "JOB_INVALID_LABEL_NAME";
    
    /// <summary>
    /// The label name exists
    /// </summary>
    public const string EXISTING_LABEL_NAME = "JOB_EXISTING_LABEL_NAME";
    
    /// <summary>
    /// The unknown error
    /// </summary>
    public const string UNKNOWN_ERROR = "JOB_UNKNOWN_ERROR";
}
