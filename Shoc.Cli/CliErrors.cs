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
        /// The project is missing
        /// </summary>
        public const string MISSING_PROJECT = "CLI_MISSING_PROJECT";
        
        /// <summary>
        /// The unknown error
        /// </summary>
        public const string UNKNOWN_ERROR = "CLI_UNKNOWN_ERROR";
    }
}