
const errors = {
    "UNKNOWN_ERROR": "A system error, please try again later",
    "IDENTITY_EMPTY_EMAIL": "The email is empty",
    "IDENTITY_EMPTY_PASSWORD": "The password is empty",
    "IDENTITY_INVALID_EMAIL": "The email address is invalid",
    "IDENTITY_EXISTING_EMAIL": "The email is already registered",
    "IDENTITY_TAKEN_USERNAME": "The username is taken",
    "IDENTITY_WEAK_PASSWORD": "The password is too weak",
    "IDENTITY_INVALID_CREDENTIALS": "The email or password are invalid",
    "IDENTITY_UNVERIFIED_EMAIL": "The email is not verified",
    "IDENTITY_MISSING_DATA": "The data is missing",
    "IDENTITY_USER_LOCKED": "The user is locked",
    "IDENTITY_HOTLINK_SIGNOUT": "The sign-out is prevented",
    "IDENTITY_UNSUPPORTED_CONFIRMATION": "The confirmation method is not supported",
    "IDENTITY_EMAIL_ALREADY_CONFIRMED": "The email is already confirmed",
    "IDENTITY_NO_USER": "The user is not found",
    "IDENTITY_CONFIRMATION_REQUESTS_EXCEEDED": "Too many requests of confirmation code",
    "IDENTITY_INVALID_CONFIRMATION_CODE": "The confirmation code is not valid",
    "IDENTITY_OTP_REQUESTS_EXCEEDED": "Too many requestsof one-time passwords",
    "IDENTITY_INVALID_DELIVERY_METHOD": "The code delivery method is not valid",
    "IDENTITY_INVALID_AUTHENTICATION_METHOD": "The authentication method is not valid",
    "IDENTITY_SIGNUP_DISABLED": "The sign-up is disabled",
    "IDENTITY_UNKNOWN_ERROR": "A system error, please try again later"
}

export const resolveError = code => errors[code] || code;