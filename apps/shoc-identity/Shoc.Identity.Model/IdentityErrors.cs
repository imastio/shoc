namespace Shoc.Identity.Model
{
    /// <summary>
    /// The connect errors
    /// </summary>
    public static class IdentityErrors
    {
        /// <summary>
        /// The email address is empty
        /// </summary>
        public const string EMPTY_EMAIL = "CONNECT_EMPTY_EMAIL";

        /// <summary>
        /// The password is empty
        /// </summary>
        public const string EMPTY_PASSWORD = "CONNECT_EMPTY_PASSWORD";

        /// <summary>
        /// The password does not match confirmation
        /// </summary>
        public const string MISMATCH_PASSWORD = "CONNECT_MISMATCH_PASSWORD";

        /// <summary>
        /// The email address is invalid
        /// </summary>
        public const string INVALID_EMAIL = "CONNECT_INVALID_EMAIL";

        /// <summary>
        /// The email already exists
        /// </summary>
        public const string EXISTING_EMAIL = "CONNECT_EXISTING_EMAIL";

        /// <summary>
        /// The username is already taken
        /// </summary>
        public const string TAKEN_USERNAME = "CONNECT_TAKEN_USERNAME";

        /// <summary>
        /// The given password is weak
        /// </summary>
        public const string WEAK_PASSWORD = "CONNECT_WEAK_PASSWORD";

        /// <summary>
        /// The sign-in credentials are invalid
        /// </summary>
        public const string INVALID_CREDENTIALS = "CONNECT_INVALID_CREDENTIALS";

        /// <summary>
        /// The email is not verified
        /// </summary>
        public const string UNVERIFIED_EMAIL = "CONNECT_UNVERIFIED_EMAIL";

        /// <summary>
        /// The missing input data error
        /// </summary>
        public const string MISSING_DATA = "CONNECT_MISSING_DATA";

        /// <summary>
        /// The user is locked
        /// </summary>
        public const string USER_LOCKED = "CONNECT_USER_LOCKED";
        
        /// <summary>
        /// The hot-link signout error
        /// </summary>
        public const string HOTLINK_SIGNOUT = "CONNECT_HOTLINK_SIGNOUT";

        /// <summary>
        /// The confirmation not supported 
        /// </summary>
        public const string UNSUPPORTED_CONFIRMATION = "CONNECT_UNSUPPORTED_CONFIRMATION";

        /// <summary>
        /// The email is already confirmed
        /// </summary>
        public const string EMAIL_ALREADY_CONFIRMED = "CONNECT_EMAIL_ALREADY_CONFIRMED";

        /// <summary>
        /// The user not found error
        /// </summary>
        public const string NO_USER = "CONNECT_NO_USER";

        /// <summary>
        /// The confirmation requests exceeded
        /// </summary>
        public const string CONFIRMATION_REQUESTS_EXCEEDED = "CONNECT_CONFIRMATION_REQUESTS_EXCEEDED";

        /// <summary>
        /// The confirmation codes is invalid
        /// </summary>
        public const string INVALID_CONFIRMATION_CODE = "CONNECT_INVALID_CONFIRMATION_CODE";

        /// <summary>
        /// The one-time password requests exceeded
        /// </summary>
        public const string OTP_REQUESTS_EXCEEDED = "CONNECT_OTP_REQUESTS_EXCEEDED";

        /// <summary>
        /// The invalid delivery method
        /// </summary>
        public const string INVALID_DELIVERY_METHOD = "CONNECT_INVALID_DELIVERY_METHOD";

        /// <summary>
        /// The password recovery requests exceeded
        /// </summary>
        public const string PASSWORD_RECOVERY_REQUESTS_EXCEEDED = "CONNECT_PASSWORD_RECOVERY_REQUESTS_EXCEEDED";

        /// <summary>
        /// The password recovery code is invalid
        /// </summary>
        public const string INVALID_PASSWORD_RECOVERY_CODE = "CONNECT_INVALID_PASSWORD_RECOVERY_CODE";

        /// <summary>
        /// The invalid authentication method
        /// </summary>
        public const string INVALID_AUTHENTICATION_METHOD = "CONNECT_INVALID_AUTHENTICATION_METHOD";

        /// <summary>
        /// The sign-up is disabled
        /// </summary>
        public const string SIGNUP_DISABLED = "CONNECT_SIGNUP_DISABLED";

        /// <summary>
        /// The root removal is invalid
        /// </summary>
        public const string INVALID_ROOT_REMOVAL = "CONNECT_INVALID_ROOT_REMOVAL";

        /// <summary>
        /// The root upgrade or downgrade is disable
        /// </summary>
        public const string INVALID_ROOT_TYPE = "CONNECT_INVALID_ROOT_TYPE";

        /// <summary>
        /// The given user type is invalid
        /// </summary>
        public const string INVALID_USER_TYPE = "CONNECT_INVALID_USER_TYPE";

        /// <summary>
        /// The given user state is invalid
        /// </summary>
        public const string INVALID_USER_STATE = "CONNECT_INVALID_USER_STATE";

        /// <summary>
        /// The group name is invalid or missing
        /// </summary>
        public const string INVALID_GROUP_NAME = "CONNECT_INVALID_GROUP_NAME";

        /// <summary>
        /// The privilege name is invalid or missing.
        /// </summary>
        public const string INVALID_PRIVILEGE_NAME = "CONNECT_INVALID_PRIVILEGE_NAME";

        /// <summary>
        /// The privilege category is invalid or missing.
        /// </summary>
        public const string INVALID_PRIVILEGE_CATEGORY = "CONNECT_INVALID_PRIVILEGE_CATEGORY";

        /// <summary>
        /// The privilege description is invalid or missing.
        /// </summary>
        public const string INVALID_PRIVILEGE_DESCRIPTION = "CONNECT_INVALID_PRIVILEGE_DESCRIPTION";

        /// <summary>
        /// The role name is invalid or missing.
        /// </summary>
        public const string INVALID_ROLE_NAME = "CONNECT_INVALID_ROLE_NAME";

        /// <summary>
        /// The role description is invalid or missing.
        /// </summary>
        public const string INVALID_ROLE_DESCRIPTION = "CONNECT_INVALID_ROLE_DESCRIPTION";

        /// <summary>
        /// The group with the given name already exists
        /// </summary>
        public const string EXISTING_GROUP_NAME = "CONNECT_EXISTING_GROUP_NAME";

        /// <summary>
        /// The privilege with the given name already exists.
        /// </summary>
        public const string EXISTING_PRIVILEGE_NAME = "CONNECT_EXISTING_PRIVILEGE_NAME";

        /// <summary>
        /// The role with the given name already exists.
        /// </summary>
        public const string EXISTING_ROLE_NAME = "CONNECT_EXISTING_ROLE_NAME";

        /// <summary>
        /// The user is already a member of the given role.
        /// </summary>
        public const string EXISTING_ROLE_USER = "CONNECT_EXISTING_ROLE_USER";

        /// <summary>
        /// The user is already a member of the given group.
        /// </summary>
        public const string EXISTING_GROUP_USER = "CONNECT_EXISTING_GROUP_USER";

        /// <summary>
        /// The unknown error
        /// </summary>
        public const string UNKNOWN_ERROR = "CONNECT_UNKNOWN_ERROR";
    }
}