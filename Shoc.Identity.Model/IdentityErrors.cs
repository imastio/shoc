namespace Shoc.Identity.Model
{
    /// <summary>
    /// The set of identity management errors
    /// </summary>
    public static class IdentityErrors
    {
        /// <summary>
        /// The email address is empty
        /// </summary>
        public const string EMPTY_EMAIL = "IDENTITY_EMPTY_EMAIL";

        /// <summary>
        /// The password is empty
        /// </summary>
        public const string EMPTY_PASSWORD = "IDENTITY_EMPTY_PASSWORD";

        /// <summary>
        /// The email address is invalid
        /// </summary>
        public const string INVALID_EMAIL = "IDENTITY_INVALID_EMAIL";

        /// <summary>
        /// The email already exists
        /// </summary>
        public const string EXISTING_EMAIL = "IDENTITY_EXISTING_EMAIL";

        /// <summary>
        /// The username is already taken
        /// </summary>
        public const string TAKEN_USERNAME = "IDENTITY_TAKEN_USERNAME";

        /// <summary>
        /// The given password is weak
        /// </summary>
        public const string WEAK_PASSWORD = "IDENTITY_WEAK_PASSWORD";

        /// <summary>
        /// The sign-in credentials are invalid
        /// </summary>
        public const string INVALID_CREDENTIALS = "IDENTITY_INVALID_CREDENTIALS";

        /// <summary>
        /// The email is not verified
        /// </summary>
        public const string UNVERIFIED_EMAIL = "IDENTITY_UNVERIFIED_EMAIL";

        /// <summary>
        /// The missing input data error
        /// </summary>
        public const string MISSING_DATA = "IDENTITY_MISSING_DATA";

        /// <summary>
        /// The user is locked
        /// </summary>
        public const string USER_LOCKED = "IDENTITY_USER_LOCKED";

        /// <summary>
        /// The hot-link signout error
        /// </summary>
        public const string HOTLINK_SIGNOUT = "IDENTITY_HOTLINK_SIGNOUT";
        
        /// <summary>
        /// The sign-up is disabled
        /// </summary>
        public const string SIGNUP_DISABLED = "IDENTITY_SIGNUP_DISABLED";

        /// <summary>
        /// The user not found error
        /// </summary>
        public const string NO_USER = "IDENTITY_NO_USER";

        /// <summary>
        /// The email is already confirmed
        /// </summary>
        public const string EMAIL_ALREADY_CONFIRMED = "IDENTITY_EMAIL_ALREADY_CONFIRMED";

        /// <summary>
        /// The confirmation requests exceeded
        /// </summary>
        public const string CONFIRMATION_REQUESTS_EXCEEDED = "IDENTITY_CONFIRMATION_REQUESTS_EXCEEDED";

        /// <summary>
        /// The confirmation codes is invalid
        /// </summary>
        public const string INVALID_CONFIRMATION_CODE = "IDENTITY_INVALID_CONFIRMATION_CODE";

        /// <summary>
        /// The unknown error
        /// </summary>
        public const string UNKNOWN_ERROR = "IDENTITY_UNKNOWN_ERROR";
    }
}