namespace Shoc.Cli
{
    /// <summary>
    /// The command line errors
    /// </summary>
    public static class CliErrors
    {
        /// <summary>
        /// The profile is invalid
        /// </summary>
        public const string INVALID_PROFILE = "CLI_INVALID_PROFILE";

        /// <summary>
        /// The authority is invalid
        /// </summary>
        public const string INVALID_AUTHORITY = "CLI_INVALID_AUTHORITY";

        /// <summary>
        /// The login failed
        /// </summary>
        public const string LOGIN_FAILED = "CLI_LOGIN_FAILED";

        /// <summary>
        /// The user is not logged in
        /// </summary>
        public const string NOT_LOGGED_IN = "CLI_NOT_LOGGED_IN";

        /// <summary>
        /// The manifest is missing
        /// </summary>
        public const string MISSING_MANIFEST = "CLI_MISSING_MANIFEST";

        /// <summary>
        /// The manifest build input is missing
        /// </summary>
        public const string MISSING_MANIFEST_BUILD_INPUT = "CLI_MISSING_MANIFEST_BUILD_INPUT";

        /// <summary>
        /// The required file is missing
        /// </summary>
        public const string MISSING_REQUIRED_FILES = "CLI_MISSING_REQUIRED_FILES";

        /// <summary>
        /// The file is outside of the context
        /// </summary>
        public const string FILE_OUTSIDE_CONTEXT = "CLI_FILE_OUTSIDE_CONTEXT";
        
        /// <summary>
        /// The project is missing
        /// </summary>
        public const string MISSING_PROJECT = "CLI_MISSING_PROJECT";

        /// <summary>
        /// The unknown error
        /// </summary>
        public const string UNKNOWN_ERROR = "CLI_UNKNOWN_ERROR";

        /// <summary>
        /// The registry name error
        /// </summary>
        public const string REGISTRY_NAME_ERROR = "CLI_REGISTRY_NAME_INVALID";

        /// <summary>
        /// The registry uri error
        /// </summary>
        public const string REGISTRY_REGISTRY_URI_ERROR = "CLI_REGISTRY_REGISTRY_URI_INVALID";

        /// <summary>
        /// The registry repository error
        /// </summary>
        public const string REGISTRY_REPOSITORY_ERROR = "CLI_REGISTRY_REPOSITORY_INVALID";

        /// <summary>
        /// The cluster name error
        /// </summary>
        public const string CLUSTER_NAME_ERROR = "CLI_CLUSTER_NAME_INVALID";

        /// <summary>
        /// The cluster kubeconfig not specified error
        /// </summary>
        public const string CLUSTER_KUBECONFIG_PATH_ERROR = "CLI_CLUSTER_KUBECONFIG_PATH_INVALID";

        /// <summary>
        /// The cluster kubeconfig file empty error
        /// </summary>
        public const string CLUSTER_KUBECONFIG_EMPTY_ERROR = "CLI_CLUSTER_KUBECONFIG_FILE_INVALID";

        /// <summary>
        /// The cluster api server uri error
        /// </summary>
        public const string CLUSTER_URI_ERROR = "CLI_CLUSTER_URI_INVALID";

        /// <summary>
        /// The project version name error
        /// </summary>
        public const string PROJECT_VERSION_ERROR = "CLI_PROJECT_VERSION_INVALID";

        /// <summary>
        /// The project name is missing
        /// </summary>
        public const string MISSING_PROJECT_NAME = "CLI_MISSING_PROJECT_NAME";

        /// <summary>
        /// The project directory is missing
        /// </summary>
        public const string MISSING_PROJECT_DIRECTORY = "CLI_MISSING_PROJECT_DIRECTORY";

        /// <summary>
        /// The project type is missing
        /// </summary>
        public const string MISSING_PROJECT_TYPE = "CLI_MISSING_PROJECT_TYPE";
        
        /// <summary>
        /// The project is invalid
        /// </summary>
        public const string INVALID_PROJECT = "CLI_INVALID_PROJECT";

        /// <summary>
        /// The job failed to create
        /// </summary>
        public const string JOB_CREATE_FAILED = "CLI_JOB_CREATE_FAILED";

        /// <summary>
        /// The job invalid
        /// </summary>
        public const string JOB_INVALID = "CLI_JOB_INVALID";
    }
}