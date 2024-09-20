namespace Shoc.Secret.Model;

/// <summary>
/// The secret errors
/// </summary>
public static class SecretErrors
{   
    /// <summary>
    /// The name is invalid
    /// </summary>
    public const string INVALID_NAME = "SECRET_INVALID_NAME";

    /// <summary>
    /// The description is invalid
    /// </summary>
    public const string INVALID_DESCRIPTION = "SECRET_INVALID_DESCRIPTION";
    
    /// <summary>
    /// The name already exists
    /// </summary>
    public const string EXISTING_NAME = "SECRET_EXISTING_NAME";
    
    /// <summary>
    /// The workspace is invalid
    /// </summary>
    public const string INVALID_WORKSPACE = "SECRET_INVALID_WORKSPACE";
       
    /// <summary>
    /// The user is invalid
    /// </summary>
    public const string INVALID_USER = "SECRET_INVALID_USER";
    
    /// <summary>
    /// The unknown error
    /// </summary>
    public const string UNKNOWN_ERROR = "SECRET_UNKNOWN_ERROR";
}
