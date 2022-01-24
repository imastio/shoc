
const errors = {
    "UNKNOWN_ERROR": "A system error, please try again later",
    "CONNECT_EMPTY_EMAIL": "The email is empty",
    "CONNECT_EMPTY_PASSWORD": "The password is empty",
    "CONNECT_INVALID_EMAIL": "The email address is invalid",
    "CONNECT_EXISTING_EMAIL": "The email is already registered",
    "CONNECT_TAKEN_USERNAME": "The username is taken",
    "CONNECT_WEAK_PASSWORD": "The password is too weak",
    "CONNECT_INVALID_CREDENTIALS": "The email or password are invalid",
    "CONNECT_UNVERIFIED_EMAIL": "The email is not verified",
    "CONNECT_MISSING_DATA": "The data is missing",
    "CONNECT_USER_LOCKED": "The user is locked",
    "CONNECT_HOTLINK_SIGNOUT": "The sign-out is prevented",
    "CONNECT_UNSUPPORTED_CONFIRMATION": "The confirmation method is not supported",
    "CONNECT_EMAIL_ALREADY_CONFIRMED": "The email is already confirmed",
    "CONNECT_NO_USER": "The user is not found",
    "CONNECT_CONFIRMATION_REQUESTS_EXCEEDED": "Too many requests of confirmation code",
    "CONNECT_INVALID_CONFIRMATION_CODE": "The confirmation code is not valid",
    "CONNECT_OTP_REQUESTS_EXCEEDED": "Too many requestsof one-time passwords",
    "CONNECT_INVALID_DELIVERY_METHOD": "The code delivery method is not valid",
    "CONNECT_INVALID_AUTHENTICATION_METHOD": "The authentication method is not valid",
    "CONNECT_SIGNUP_DISABLED": "The sign-up is disabled",
    "CONNECT_UNKNOWN_ERROR": "A system error, please try again later"
}

export const resolveError = code => errors[code] || code;