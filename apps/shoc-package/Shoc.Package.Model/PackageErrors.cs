namespace Shoc.Package.Model;

/// <summary>
/// The package errors
/// </summary>
public static class PackageErrors
{   
    /// <summary>
    /// The workspace is invalid
    /// </summary>
    public const string INVALID_WORKSPACE = "PACKAGE_INVALID_WORKSPACE";
    
    /// <summary>
    /// The user is invalid
    /// </summary>
    public const string INVALID_USER = "PACKAGE_INVALID_USER";

    /// <summary>
    /// The build provider is invalid
    /// </summary>
    public const string INVALID_BUILD_PROVIDER = "PACKAGE_INVALID_BUILD_PROVIDER";

    /// <summary>
    /// The listing checksum is invalid
    /// </summary>
    public const string INVALID_LISTING_CHECKSUM = "PACKAGE_INVALID_LISTING_CHECKSUM";

    /// <summary>
    /// The package scope is invalid
    /// </summary>
    public const string INVALID_PACKAGE_SCOPE = "PACKAGE_INVALID_PACKAGE_SCOPE";

    /// <summary>
    /// The manifest is invalid
    /// </summary>
    public const string INVALID_MANIFEST = "PACKAGE_INVALID_MANIFEST";
    
    /// <summary>
    /// The template is invalid
    /// </summary>
    public const string INVALID_TEMPLATE = "PACKAGE_INVALID_TEMPLATE";
    
    /// <summary>
    /// The build spec is invalid
    /// </summary>
    public const string INVALID_BUILD_SPEC = "PACKAGE_INVALID_BUILD_SPEC";
    
    /// <summary>
    /// The runtime is invalid
    /// </summary>
    public const string INVALID_RUNTIME = "PACKAGE_INVALID_RUNTIME";
    
    /// <summary>
    /// The template is invalid
    /// </summary>
    public const string INVALID_CONTAINERFILE_TEMPLATE = "PACKAGE_INVALID_CONTAINERFILE_TEMPLATE";

    /// <summary>
    /// The error indicating render failure
    /// </summary>
    public const string RENDER_FAILURE = "PACKAGE_RENDER_FAILURE";

    /// <summary>
    /// The registry is invalid
    /// </summary>
    public const string INVALID_REGISTRY = "PACKAGE_INVALID_REGISTRY";
    
    /// <summary>
    /// The registry credential is invalid or not found
    /// </summary>
    public const string INVALID_REGISTRY_CREDENTIALS = "PACKAGE_INVALID_REGISTRY_CREDENTIALS";
    
    /// <summary>
    /// The user mismatch
    /// </summary>
    public const string USER_MISMATCH = "PACKAGE_USER_MISMATCH";
    
    /// <summary>
    /// The expired operation
    /// </summary>
    public const string UNEXPECTED_OPERATION = "PACKAGE_UNEXPECTED_OPERATION";
    
    /// <summary>
    /// The build task is expired
    /// </summary>
    public const string EXPIRED_BUILD_TASK = "PACKAGE_EXPIRED_BUILD_TASK";
    
    /// <summary>
    /// The bundle type is invalid
    /// </summary>
    public const string INVALID_BUNDLE_TYPE = "PACKAGE_INVALID_BUNDLE_TYPE";
    
    /// <summary>
    /// The bundle upload error
    /// </summary>
    public const string UPLOAD_ERROR = "PACKAGE_UPLOAD_ERROR";
    
    /// <summary>
    /// The bundle unzip error
    /// </summary>
    public const string UNZIP_ERROR = "PACKAGE_UNZIP_ERROR";
    
    /// <summary>
    /// The image build error
    /// </summary>
    public const string IMAGE_BUILD_ERROR = "PACKAGE_IMAGE_BUILD_ERROR";
    
    /// <summary>
    /// The image push error
    /// </summary>
    public const string IMAGE_PUSH_ERROR = "PACKAGE_IMAGE_PUSH_ERROR";
    
    /// <summary>
    /// The unknown error
    /// </summary>
    public const string UNKNOWN_ERROR = "PACKAGE_UNKNOWN_ERROR";
}
