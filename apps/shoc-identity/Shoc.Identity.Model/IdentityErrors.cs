namespace Shoc.Identity.Model;

/// <summary>
/// The identity errors
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
    /// The password does not match confirmation
    /// </summary>
    public const string MISMATCH_PASSWORD = "IDENTITY_MISMATCH_PASSWORD";

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
    /// The confirmation not supported 
    /// </summary>
    public const string UNSUPPORTED_CONFIRMATION = "IDENTITY_UNSUPPORTED_CONFIRMATION";

    /// <summary>
    /// The email is already confirmed
    /// </summary>
    public const string EMAIL_ALREADY_CONFIRMED = "IDENTITY_EMAIL_ALREADY_CONFIRMED";

    /// <summary>
    /// The user not found error
    /// </summary>
    public const string NO_USER = "IDENTITY_NO_USER";

    /// <summary>
    /// The confirmation requests exceeded
    /// </summary>
    public const string CONFIRMATION_REQUESTS_EXCEEDED = "IDENTITY_CONFIRMATION_REQUESTS_EXCEEDED";

    /// <summary>
    /// The confirmation codes is invalid
    /// </summary>
    public const string INVALID_CONFIRMATION_CODE = "IDENTITY_INVALID_CONFIRMATION_CODE";

    /// <summary>
    /// The one-time password requests exceeded
    /// </summary>
    public const string OTP_REQUESTS_EXCEEDED = "IDENTITY_OTP_REQUESTS_EXCEEDED";

    /// <summary>
    /// The invalid delivery method
    /// </summary>
    public const string INVALID_DELIVERY_METHOD = "IDENTITY_INVALID_DELIVERY_METHOD";

    /// <summary>
    /// The password recovery requests exceeded
    /// </summary>
    public const string PASSWORD_RECOVERY_REQUESTS_EXCEEDED = "IDENTITY_PASSWORD_RECOVERY_REQUESTS_EXCEEDED";

    /// <summary>
    /// The password recovery code is invalid
    /// </summary>
    public const string INVALID_PASSWORD_RECOVERY_CODE = "IDENTITY_INVALID_PASSWORD_RECOVERY_CODE";

    /// <summary>
    /// The invalid authentication method
    /// </summary>
    public const string INVALID_AUTHENTICATION_METHOD = "IDENTITY_INVALID_AUTHENTICATION_METHOD";

    /// <summary>
    /// The sign-up is disabled
    /// </summary>
    public const string SIGNUP_DISABLED = "IDENTITY_SIGNUP_DISABLED";

    /// <summary>
    /// The root removal is invalid
    /// </summary>
    public const string INVALID_ROOT_REMOVAL = "IDENTITY_INVALID_ROOT_REMOVAL";

    /// <summary>
    /// The root upgrade or downgrade is disable
    /// </summary>
    public const string INVALID_ROOT_TYPE = "IDENTITY_INVALID_ROOT_TYPE";

    /// <summary>
    /// The given user type is invalid
    /// </summary>
    public const string INVALID_USER_TYPE = "IDENTITY_INVALID_USER_TYPE";

    /// <summary>
    /// The given user state is invalid
    /// </summary>
    public const string INVALID_USER_STATE = "IDENTITY_INVALID_USER_STATE";

    /// <summary>
    /// The group name is invalid or missing
    /// </summary>
    public const string INVALID_GROUP_NAME = "IDENTITY_INVALID_GROUP_NAME";

    /// <summary>
    /// The privilege name is invalid or missing.
    /// </summary>
    public const string INVALID_PRIVILEGE_NAME = "IDENTITY_INVALID_PRIVILEGE_NAME";

    /// <summary>
    /// The privilege category is invalid or missing.
    /// </summary>
    public const string INVALID_PRIVILEGE_CATEGORY = "IDENTITY_INVALID_PRIVILEGE_CATEGORY";

    /// <summary>
    /// The privilege description is invalid or missing.
    /// </summary>
    public const string INVALID_PRIVILEGE_DESCRIPTION = "IDENTITY_INVALID_PRIVILEGE_DESCRIPTION";

    /// <summary>
    /// The role name is invalid or missing.
    /// </summary>
    public const string INVALID_ROLE_NAME = "IDENTITY_INVALID_ROLE_NAME";

    /// <summary>
    /// The role description is invalid or missing.
    /// </summary>
    public const string INVALID_ROLE_DESCRIPTION = "IDENTITY_INVALID_ROLE_DESCRIPTION";

    /// <summary>
    /// The group with the given name already exists
    /// </summary>
    public const string EXISTING_GROUP_NAME = "IDENTITY_EXISTING_GROUP_NAME";

    /// <summary>
    /// The privilege with the given name already exists.
    /// </summary>
    public const string EXISTING_PRIVILEGE_NAME = "IDENTITY_EXISTING_PRIVILEGE_NAME";

    /// <summary>
    /// The role with the given name already exists.
    /// </summary>
    public const string EXISTING_ROLE_NAME = "IDENTITY_EXISTING_ROLE_NAME";

    /// <summary>
    /// The user is already a member of the given role.
    /// </summary>
    public const string EXISTING_ROLE_USER = "IDENTITY_EXISTING_ROLE_USER";

    /// <summary>
    /// The user is already a member of the given group.
    /// </summary>
    public const string EXISTING_GROUP_USER = "IDENTITY_EXISTING_GROUP_USER";

    /// <summary>
    /// The provider name is invalid
    /// </summary>
    public const string INVALID_PROVIDER_NAME = "IDENTITY_INVALID_PROVIDER_NAME";
    
    /// <summary>
    /// The provider code is invalid
    /// </summary>
    public const string INVALID_PROVIDER_CODE = "IDENTITY_INVALID_PROVIDER_CODE";
    
    /// <summary>
    /// The provider code is invalid
    /// </summary>
    public const string EXISTING_PROVIDER_CODE = "IDENTITY_EXISTING_PROVIDER_CODE";
    
    /// <summary>
    /// The provider type is invalid
    /// </summary>
    public const string INVALID_PROVIDER_TYPE = "IDENTITY_INVALID_PROVIDER_TYPE";
    
    /// <summary>
    /// The provider scope is invalid
    /// </summary>
    public const string INVALID_PROVIDER_SCOPE = "IDENTITY_INVALID_PROVIDER_SCOPE";
    
    /// <summary>
    /// The provider client id is invalid
    /// </summary>
    public const string INVALID_PROVIDER_CLIENT_ID = "IDENTITY_INVALID_PROVIDER_CLIENT_ID";
    
    /// <summary>
    /// The provider client secret is invalid
    /// </summary>
    public const string INVALID_PROVIDER_CLIENT_SECRET = "IDENTITY_INVALID_PROVIDER_CLIENT_SECRET";

    /// <summary>
    /// The provider icon url is invalid
    /// </summary>
    public const string INVALID_PROVIDER_ICON_URL = "IDENTITY_INVALID_PROVIDER_ICON_URL";
    
    /// <summary>
    /// The provider domain name is invalid
    /// </summary>
    public const string INVALID_PROVIDER_DOMAIN_NAME = "IDENTITY_INVALID_PROVIDER_DOMAIN_NAME";
    
    /// <summary>
    /// The provider domain name already exists
    /// </summary>
    public const string EXISTING_PROVIDER_DOMAIN_NAME = "IDENTITY_EXISTING_PROVIDER_DOMAIN_NAME";
    
    /// <summary>
    /// The provider authority is invalid
    /// </summary>
    public const string INVALID_PROVIDER_AUTHORITY = "IDENTITY_INVALID_PROVIDER_AUTHORITY";
    
    /// <summary>
    /// The unknown error
    /// </summary>
    public const string UNKNOWN_ERROR = "IDENTITY_UNKNOWN_ERROR";
}
